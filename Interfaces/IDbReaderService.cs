namespace DelayedDataLoading;

internal interface IDbReaderService
{
    Task<IEnumerable<dynamic>> ReadDataAsync(string command, Dictionary<string, object> dictParams);
}