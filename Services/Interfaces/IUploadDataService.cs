namespace Analysis.Animal.System.Services.Interfaces
{
    public interface IUploadDataService
    {
        /// <summary>
        /// Faz upload e importação da planilha com os dados de suporte
        /// </summary>
        string UploadSupportSheet(IFormFile formFile);

        /// <summary>
        /// Faz upload e importação do arquivo com as fazendas
        /// </summary>
        string UploadFarm(IFormFile formFile);
    }
}