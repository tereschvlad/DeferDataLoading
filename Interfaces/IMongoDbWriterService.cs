namespace DelayedDataLoading;

internal interface IMongoDbWriterService
{
    Task WriteDataAsync();
}