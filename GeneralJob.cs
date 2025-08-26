using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace DelayedDataLoading;

[DisallowConcurrentExecution]
internal class GeneralJob : IJob
{
    private readonly ILogger<GeneralJob> _logger;
    private readonly IReaderService _readerService;

    private readonly IMongoDbWriterService _mongoDbWriterService;

    public GeneralJob(ILogger<GeneralJob> logger, IReaderService readerService, IMongoDbWriterService mongoDbWriterService)
    {
        _logger = logger;
        _readerService = readerService;

        _mongoDbWriterService = mongoDbWriterService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _readerService.ReadDataAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test error");
        }
    }
}
