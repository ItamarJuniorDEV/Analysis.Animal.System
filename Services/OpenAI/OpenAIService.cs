using System.Text;
using System.Text.Json;
using Analysis.Animal.System.Models;
using Analysis.Animal.System.Services.Interfaces;
using OpenAI.Chat;

namespace Analysis.Animal.System.Services.OpenAI
{
    public partial class OpenAIService : IOpenAIService
    {
        private ChatClient _openAiClient;
        private string? _apiKey;

        private HttpClient _assistantHttpClient = new();

        private static string? _analyticsCode;

        public OpenAIService()
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            _openAiClient = new ChatClient("gpt4-mini", _apiKey);

            // Configura cliente do assistente
            _assistantHttpClient.BaseAddress = new Uri("https://api.openai.com");

            _assistantHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _assistantHttpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");
        }
    }
}