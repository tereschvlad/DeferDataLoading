using Microsoft.Extensions.Logging;
using Quartz;

namespace DelayedDataLoading;

internal class GeneralJob : IJob
{
    private readonly ILogger<GeneralJob> _logger;
    private readonly IDbReaderService _dbReaderService;

    public GeneralJob(ILogger<GeneralJob> logger, IDbReaderService dbReaderService)
    {
        _logger = logger;
        _dbReaderService = dbReaderService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _dbReaderService.ReadDataAsync("SELECT * FROM example_table", string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test error");
        }
    }
}
