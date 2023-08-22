using CRM.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                string endpoint = _configuration["ChatGpt:endpoint-text-davinci-003"] ?? "";
                string instructions = _configuration["ChatGpt:instructions1"] ?? "";
                var prompt = instructions + "\n\n" + contentBody;

                var request = new ContentChatGptRequest
                {
                    prompt = prompt,
                    temperature = 1,
                    max_tokens = 150
                };

                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(result);

                    string generatedText = responseObject["choices"][0]["text"].ToString();
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
