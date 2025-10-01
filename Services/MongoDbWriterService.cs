using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DeferDataLoading;

internal class MongoDbWriterService : IMongoDbWriterService
{
    private readonly ILogger<MongoDbWriterService> _logger;
    private readonly IMongoDatabase _mongoDb;
    public MongoDbWriterService(ILogger<MongoDbWriterService> logger, IMongoDatabase mongoDb)
    {
        _logger = logger;
        _mongoDb = mongoDb;
    }

    public async Task WriteDataAsync(ResultRequestDataModel data)
    {
        try
        {
            var collection = _mongoDb.GetCollection<ResultRequestDataModel>(data.MongoCollectionName);
            await collection.InsertOneAsync(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing data to MongoDB");
        }
    }
}