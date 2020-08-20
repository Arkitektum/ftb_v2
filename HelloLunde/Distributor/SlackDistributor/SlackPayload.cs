using System.Text.Json.Serialization;

namespace Distributor
{
    public class SlackPayload
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}