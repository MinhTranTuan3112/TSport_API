using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TSport.Api.Shared
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                // Load the object from the reader and parse the properties
                var jObject = JObject.Load(reader);
                var year = (jObject["year"] is not null) ? (int)jObject["year"]! : 0;
                var month = (jObject["month"] is not null) ? (int)jObject["month"]! : 0;
                var day = (jObject["day"] is not null) ? (int)jObject["day"]! : 0;

                return new DateOnly(year, month, day);
            }

            if (reader.TokenType == JsonToken.String)
            {
                var dateString = reader.Value as string;
                return (dateString is not null) ? DateOnly.Parse(dateString) : null;
            }

            throw new JsonSerializationException($"Unexpected token parsing DateOnly. Expected String or Null, got {reader.TokenType}.");
        }

        public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(value.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}