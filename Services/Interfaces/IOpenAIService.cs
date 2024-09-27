namespace Analysis.Animal.System.Services.Interfaces
{
    public interface IOpenAIService
    {
        /// <summary>
        /// Gera o html com base no prompt
        /// </summary>
        /// <param name="prompt"></param>
        public string GenerateHtml(string prompt);
    }
}