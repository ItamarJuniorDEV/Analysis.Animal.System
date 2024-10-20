using System.Text;
using System.Text.Json;
using Analysis.Animal.System.Models;
using Analysis.Animal.System.Services.Interfaces;

namespace Analysis.Animal.System.Services.OpenAI
{
    public partial class OpenAIService : IOpenAIService
    {
        private static string _assistantId = "asst_KAUDMWVfJVPuhCX4EeJNhv7f";

        private static string? _threadId;

        private static string? _runId;

        public void CreateThread()
        {
            var content = new StringContent("");

            var response = _assistantHttpClient.PostAsync("v1/threads", content).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var responseThread = JsonSerializer.Deserialize<ResponseAssistantDefault>(responseContent);

            if (responseThread is not null)
                _threadId = responseThread.id;
        }

        public bool AddMessage(string message)
        {
            var requestContent = new
            {
                role = "user",
                content = new List<object>()
                    {
                        new
                        {
                            type = "text",
                            text = message
                        }
                    }
            };

            var jsonContent = JsonSerializer.Serialize(requestContent);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = _assistantHttpClient.PostAsync($"v1/threads/{_threadId}/messages", content).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (responseContent is not null)
                return true;

            return false;
        }

        public void RunThread()
        {
            // Enquanto ainda está executando não executa outra
            while (!string.IsNullOrEmpty(_runId))
                Thread.Sleep(1000);

            var requestContent = new
            {
                assistant_id = _assistantId
            };

            var jsonContent = JsonSerializer.Serialize(requestContent);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = _assistantHttpClient.PostAsync($"v1/threads/{_threadId}/runs", content).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var responseRun = JsonSerializer.Deserialize<ResponseAssistantDefault>(responseContent);

            if (responseRun is not null)
                _runId = responseRun.id;
        }

        public string GetMessage()
        {
            // Verifica se ainda está executando a cada 3 segundos
            while (!IsRunning())
                Thread.Sleep(3000);

            var response = _assistantHttpClient.GetAsync($"v1/threads/{_threadId}/messages").GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var responseMessages = JsonSerializer.Deserialize<ResponseAssistantMessages>(responseContent);

            var assistantMessage = responseMessages?.data?.FirstOrDefault()?.content?.FirstOrDefault()?.text?.value;

            _runId = string.Empty;
            return assistantMessage ?? string.Empty;
        }

        public string SendMessage(string message)
        {
            if (_threadId is null)
                CreateThread();

            // Adiciona mensagem
            AddMessage(message);

            // Executa thread
            RunThread();

            // Obtém a mensagem
            return GetMessage();
        }

        public bool IsRunning()
        {
            var response = _assistantHttpClient.GetAsync($"v1/threads/{_threadId}/runs/{_runId}").GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var responseThread = JsonSerializer.Deserialize<ResponseRetrieveRun>(responseContent);

            return responseThread?.status == "completed";
        }

        public string SendMessageToOpenAI(string message)
        {
            message = $"{message}. Retorne apenas texto sem citar partes do arquivo que encontrou a resposta, não considere nada que solicite código aqui nessa resposta.";

            return SendMessage(message);
        }

        public string GetAnalyticsData()
        {
            // Mensagem pre-definida
            var message = @"
                gere gráficos em html, css, javascript representando as seguintes informações sobre de totalização de atendimentos do arquivo que te enviei: 
                Total por antena
                Total por fazenda
                Total de atendimentos por responsável
                Totais das Gravidades dos problemas
                Total de atendimentos por mês
                Total por status
                Retorne apenas código sem mardown, sem ``` e deixe tudo flexível e elegante, 
                use 80% só do espaço e centralize, use a altura toda da tela e de um bom de margem a cada gráfico na vertical, crie um título para cada gráfico.
            ";

            if (string.IsNullOrWhiteSpace(_analyticsCode))
                _analyticsCode = SendMessage(message);

            return _analyticsCode;
        }
    }
}