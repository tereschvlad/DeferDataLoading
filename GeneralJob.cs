using Microsoft.Extensions.Logging;
using Quartz;

namespace DelayedDataLoading;

public class GeneralJob : IJob
{
    private ILogger<GeneralJob> _logger;

    public GeneralJob(ILogger<GeneralJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test error");
        }
    }
}
