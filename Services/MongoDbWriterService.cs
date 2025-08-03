using Microsoft.Extensions.Logging;

namespace DelayedDataLoading;

internal class MongoDbWriterService : IMongoDbWriterService
{
    private readonly ILogger<MongoDbWriterService> _logger;
    public MongoDbWriterService(ILogger<MongoDbWriterService> logger)
    {
        _logger = logger;
    }

    public async Task WriteDataAsync()
    {

    }
}