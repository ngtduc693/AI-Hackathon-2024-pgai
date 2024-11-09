using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceBot.Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ollamaApiUrl;

        public EmbeddingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _ollamaApiUrl = configuration.GetValue<string>("Ollama:ApiUrl");
        }

        public async Task<float[]> GetEmbeddingsAsync(string text)
        {
            var payload = new
            {
                model = "nomic-embed-text:latest",
                input = new[] { text }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_ollamaApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ollama API call failed: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(responseContent);

            var embeddings = jsonResponse["embeddings"]?.FirstOrDefault()?.ToObject<float[]>();

            if (embeddings == null)
            {
                throw new Exception("Failed to parse embeddings from Ollama API response.");
            }

            return embeddings;
        }
    }
    public class OllamaEmbeddingsResponse
    {
        [JsonProperty("embeddings")]
        public string[] Embeddings { get; set; }
    }
}
