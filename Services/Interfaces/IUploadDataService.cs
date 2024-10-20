namespace Analysis.Animal.System.Services.Interfaces
{
    public interface IUploadDataService
    {
        /// <summary>
        /// Faz upload e importação da planilha com os dados de suporte
        /// </summary>
        void GenerateAssistantData(IFormFile formFile);

        /// <summary>
        /// Faz upload e importação do arquivo com as fazendas
        /// </summary>
        string UploadFarm(IFormFile formFile);
    }
}