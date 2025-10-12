using DeferDataLoading;
using DeferDataLoading.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Quartz;
using Serilog;
using Serilog.Events;

try
{
    var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();

    Log.Information("Start hosting");

    var workerDelay = 120; // default 120 seconds

    var builder = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
    {
        services.Configure<ConnectionDataOption>(context.Configuration.GetSection("Connections"));

        var config = context.Configuration.GetSection("Connections").Get<ConnectionDataOption>();
        workerDelay = config.WorkerDelayed;

        switch (config.DbName)
        {
            case "PostgreSql":
                services.AddSingleton<IDbReaderService, PotgreeReaderService>();
                break;
            case "MySql":
                services.AddSingleton<IDbReaderService, MySqlReaderService>();
                break;
            case "Oracle":
                services.AddSingleton<IDbReaderService, OracleReaderService>();
                break;
            case "MSSQL":
                services.AddSingleton<IDbReaderService, MSSqlReaderService>();
                break;
        }

        services.AddSingleton<IReaderService, ReaderService>();
        services.AddSingleton<IMongoDbWriterService, MongoDbWriterService>();
        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var client = new MongoClient(config.MongoDbConnection);
            return client.GetDatabase(config.MongoDbName);
        });

        services.AddSerilog((serviceProv, logConf) =>
        {
            logConf.WriteTo.Console()
                   .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, 
                                restrictedToMinimumLevel: LogEventLevel.Information)
                   .WriteTo.Seq(config.SeqHost, apiKey: config.SeqKey, 
                                restrictedToMinimumLevel: LogEventLevel.Information);
        });

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
    .WithSimpleSchedule(x => x.WithIntervalInSeconds(workerDelay).RepeatForever()).Build();

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
