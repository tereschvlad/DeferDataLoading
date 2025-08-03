using Microsoft.Extensions.Logging;

namespace DelayedDataLoading;

internal class DbReaderService : IReaderService
{
    private readonly ILogger<DbReaderService> _logger;
    public DbReaderService(ILogger<DbReaderService> logger)
    {
        _logger = logger;
    }

    public async Task ReadDataAsync()
    {

    }
}
