using CRM.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CRM.Service
{
    public class ChatGptService : IChatGptService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChatGptService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();

            // Setup HttpClient headers
            string apiKey = _configuration["ChatGpt:apiKey"] ?? "";
            string orgId = _configuration["ChatGpt:orgId"] ?? "";
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", orgId);
        }

        public async Task<string> GenerateSuggestedContent(string contentBody)
        {
            try
            {
                string endpoint = _configuration["ChatGpt:endpoint-gpt-3.5-turbo"] ?? "";
                string instructions = _configuration["ChatGpt:instructions1"] ?? "";
                //var prompt = instructions + "\n\n" + contentBody;

                var request = new ContentChatGptRequest
                {
                    Model = _configuration["ChatGpt:model"] ?? "",
                    Temperature = 0.7,
                    Messages = new List<Messages>
                    {
                        new Messages
                        {
                            Role = "user",
                            Content = contentBody
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["ChatGpt:apiKey"] ?? "");
                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(result);

                    string generatedText = responseObject["choices"][0]["message"]["content"].ToString();
                    return generatedText;
                }
                else
                {
                    return "API request failed";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}