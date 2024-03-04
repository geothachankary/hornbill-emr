using System.Text.Json;
using System.Text.Json.Serialization;
using Hornbill.Emr.Api.Infrastructure.Localization;

namespace Hornbill.Emr.Api.Infrastructure.Serialization.JsonConverters;

public sealed class LocalizationConverter(IAppLocalizer localizer) : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(localizer[value]);
    }
}
