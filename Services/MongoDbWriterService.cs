using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DelayedDataLoading;

internal class MongoDbWriterService : IMongoDbWriterService
{
    private readonly ILogger<MongoDbWriterService> _logger;
    public MongoDbWriterService(ILogger<MongoDbWriterService> logger)
    {
        _logger = logger;
    }

    public async Task WriteDataAsync(ResultRequestDataModel data)
    {
        try
        {
            var client = new MongoClient("mongodb://test:test@localhost:27017/");
            var database = client.GetDatabase("DelayedDataLoading");

            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing data to MongoDB");
        }
    }
}