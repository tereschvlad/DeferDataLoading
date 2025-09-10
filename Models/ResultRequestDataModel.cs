using MongoDB.Bson;

namespace DelayedDataLoading;

internal class ResultRequestDataModel : RequestDataModel
{
    public string ResultJson { get; set; }
    
    public IEnumerable<BsonDocument> Rows { get; set; }

    public DateTime CreateDate { get; set; }
}