using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

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
            var dictParams = JsonSerializer.Deserialize<Dictionary<string, string>>("{\"Id\": \"1\"}");

            await _dbReaderService.ReadDataAsync("SELECT * FROM example_table WHERE Id = @Id", dictParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test error");
        }
    }
}
