namespace DelayedDataLoading;

internal interface IDbReaderService
{
    Task<IEnumerable<object>> ReadDataAsync(string command, Dictionary<string, object> dictParams);
}