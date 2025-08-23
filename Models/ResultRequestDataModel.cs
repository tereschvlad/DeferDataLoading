namespace DelayedDataLoading;

internal class ResultRequestDataModel : RequestDataModel
{
    public IEnumerable<object> Result { get; set; }

    public DateTime CreateDate { get; set; }
}