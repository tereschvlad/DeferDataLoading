using MongoDB.Bson;

namespace DelayedDataLoading;

internal class ResultRequestDataModel : RequestDataModel
{
    public IEnumerable<BsonDocument> Rows { get; set; }

    public DateTime CreateDate { get; set; }
}