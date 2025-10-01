using MongoDB.Bson;

namespace DeferDataLoading;

internal class ResultRequestDataModel : RequestDataModel
{
    public IEnumerable<BsonDocument> Rows { get; set; }

    public DateTime CreateDate { get; set; }
}