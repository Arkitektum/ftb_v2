using System.Text.Json.Serialization;

namespace FtB_MessageManager
{
    public class SlackPayload
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}