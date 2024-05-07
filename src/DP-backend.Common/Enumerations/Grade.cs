using System.Text.Json.Serialization;

namespace DP_backend.Common.Enumerations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Grade
    {
        First,
        Second,
        Third,
        Fourth
    }
}
