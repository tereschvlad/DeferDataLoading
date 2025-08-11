using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace DelayedDataLoading;

[DisallowConcurrentExecution]
internal class GeneralJob : IJob
{
    private readonly ILogger<GeneralJob> _logger;
    private readonly IDbReaderService _dbReaderService;

    private readonly IReaderService _readerService;

    public GeneralJob(ILogger<GeneralJob> logger, IDbReaderService dbReaderService, IReaderService readerService)
    {
        _logger = logger;
        _dbReaderService = dbReaderService;
        _readerService = readerService;
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
