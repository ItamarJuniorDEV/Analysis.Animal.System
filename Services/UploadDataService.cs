using Analysis.Animal.System.Services.Interfaces;

namespace Analysis.Animal.System.Services
{
    public class UploadDataService : IUploadDataService
    {
        public string UploadFarm(string fileName)
        {
            return $"Arquivo com as fazendo de nome {fileName} importado com sucesso.";
        }

        public string UploadSupportSheet(string fileName)
        {
            return $"Arquivo com os dados suporte de nome {fileName} importado com sucesso.";
        }
    }
}