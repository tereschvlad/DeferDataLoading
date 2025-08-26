using DelayedDataLoading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quartz;
using Serilog;

try
{
    var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
        .CreateLogger();

    Log.Information("Start hosting");

    var builder = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
    {
        services.Configure<ConnectionDataOption>(context.Configuration.GetSection("Connections"));
        services.AddSingleton<IDbReaderService, PotgreeReaderService>();
        services.AddSingleton<IReaderService, ReaderService>();
        services.AddSingleton<IMongoDbWriterService, MongoDbWriterService>();
        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IOptions<ConnectionDataOption>>();

            var client = new MongoClient(config.Value.MongoDbConnection);
            return client.GetDatabase(config.Value.MongoDbName);
        });
        services.AddSerilog();
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
    }).Build();


    var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
    var scheduler = await schedulerFactory.GetScheduler();

    var job = JobBuilder.Create<GeneralJob>()
                        .Build();

    var trigger = TriggerBuilder.Create()
    .StartNow()
    .WithSimpleSchedule(x =>
    {
        x.WithIntervalInSeconds(180).RepeatForever();
    }).Build();

    await scheduler.ScheduleJob(job, trigger);

    await builder.RunAsync();
}
catch (Exception ex)
{
    Log.Error(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
