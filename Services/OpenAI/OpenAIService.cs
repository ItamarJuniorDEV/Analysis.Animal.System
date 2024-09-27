using Analysis.Animal.System.Services.Interfaces;
using OpenAI.Chat;

namespace Analysis.Animal.System.Services.OpenAI
{
    public class OpenAIService : IOpenAIService
    {
        public string GenerateHtml(string prompt)
        {
            var client = new ChatClient("gpt-4o-mini", Environment.GetEnvironmentVariable("OPEN_AI_APIKEY"));

            var completion = client.CompleteChat(prompt);

            var html = completion.Value.Content[0].Text;

            return html;
        }
    }
}