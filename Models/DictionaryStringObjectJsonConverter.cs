using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeferDataLoading;

public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object>>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Dictionary<string, object>) == typeToConvert;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }

    public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        var dictionary = new Dictionary<string, object>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return dictionary;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName token");
            }

            string propertyName = reader.GetString();

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new JsonException("Property name is null or empty");
            }

            reader.Read();
            dictionary.Add(propertyName, ExtractValue(ref reader, options));
        }  

        return dictionary; 
    }

    private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                if (reader.TryGetDateTime(out DateTime datetime))
                {
                    return datetime;
                }
                return reader.GetString();
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Null:
                return null!;
            case JsonTokenType.Number:
                if (reader.TryGetInt64(out long l))
                    return l;
                return reader.GetDouble();
            case JsonTokenType.StartObject:
                return Read(ref reader, null, options);
            case JsonTokenType.StartArray:
                var list = new List<object>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    list.Add(ExtractValue(ref reader, options));
                }
                return list;
            default:
                throw new JsonException($"Unsupported token type: {reader.TokenType}");
        }
    }
}