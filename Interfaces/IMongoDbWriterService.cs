namespace DeferDataLoading;

internal interface IMongoDbWriterService
{
    Task WriteDataAsync(ResultRequestDataModel data);
}