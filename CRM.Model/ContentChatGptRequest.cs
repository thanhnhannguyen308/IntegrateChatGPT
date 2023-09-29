using Newtonsoft.Json;

namespace CRM.Model
{
    public class ContentChatGptRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("messages")]
        public List<Messages> Messages { get; set; }
    }

    public class Messages
    {
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }

    }
}