namespace DelayedDataLoading;

internal interface IDbReaderService
{
    Task ReadDataAsync(string command, Dictionary<string, string> dictParams);
}