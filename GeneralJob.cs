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
            #region Test code, creating json from RequestDataModel

            var prms = new RequestDataModel
            {
                Request = "SELECT * FROM example_table WHERE Id = @Id",
                Parameters = new Dictionary<string, object>
                {
                    { "Id", 1 }
                }
            };

            var json = JsonSerializer.Serialize(prms);

            #endregion

            var deserializedPrms = JsonSerializer.Deserialize<RequestDataModel>(json);
            var list = await _dbReaderService.ReadDataAsync(prms.Request, prms.Parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test error");
        }
    }
}
