using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DeferDataLoading;

internal class RequestDataModel
{
    public string Request { get; set; }

    [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
    public Dictionary<string, object> Parameters { get; set; }

    public string RequestName { get; set; }

    public string Application { get; set; }

    public string UserName { get; set; }

    [BsonIgnore]
    public string MongoCollectionName { get; set; }

}

