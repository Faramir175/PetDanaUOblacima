using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PetDanaUOblacima.Utils
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Čita string iz JSON-a i parsira ga u TimeOnly
            return TimeOnly.ParseExact(reader.GetString()!, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            // Piše TimeOnly u JSON kao string u formatu "HH:mm"
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}