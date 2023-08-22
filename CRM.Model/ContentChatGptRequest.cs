namespace CRM.Model
{
    public class ContentChatGptRequest
    {
        public string prompt { get; set; }
        public int temperature { get; set; }
        public int max_tokens { get; set; }
    }
}