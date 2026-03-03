using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportowyHub.Services.Api;

public sealed class FlexibleDecimalConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(decimal) || typeToConvert == typeof(decimal?);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(decimal))
        {
            return new DecimalConverter();
        }

        return new NullableDecimalConverter();
    }

    private sealed class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetDecimal(),
                JsonTokenType.String when decimal.TryParse(reader.GetString(), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out var value) => value,
                _ => 0m
            };
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    private sealed class NullableDecimalConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetDecimal(),
                JsonTokenType.String when decimal.TryParse(reader.GetString(), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out var value) => value,
                JsonTokenType.Null => null,
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
