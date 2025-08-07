namespace DelayedDataLoading;

internal interface IDbReaderService
{
    Task ReadDataAsync(string command, string parameters);
}