using Microsoft.Extensions.Logging;

namespace DelayedDataLoading;

internal class ReaderService : IReaderService
{
    private readonly ILogger<ReaderService> _logger;
    public ReaderService(ILogger<ReaderService> logger)
    {
        _logger = logger;
    }

    public async Task ReadDataAsync()
    {

    }
}
