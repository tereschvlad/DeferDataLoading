using DelayedDataLoading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        //.WriteTo.Seq("http://localhost:5341", apiKey: "3UQijf9hnD8Apq5KsXJb")
        .CreateLogger();

    Log.Information("Start hosting");

    var builder = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
    {
        services.AddSingleton<IReaderService, ReaderService>();
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
        x.WithIntervalInSeconds(10).RepeatForever();
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
