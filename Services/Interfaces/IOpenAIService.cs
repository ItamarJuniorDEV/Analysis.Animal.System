namespace Analysis.Animal.System.Services.Interfaces
{
    public interface IOpenAIService
    {
        string GetAnalyticsData();

        string SendMessageToOpenAI(string message);
    }
}