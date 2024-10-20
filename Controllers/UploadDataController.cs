using Analysis.Animal.System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Analysis.Animal.System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadDataController : ControllerBase
    {
        private readonly IUploadDataService _uploadDataService;

        public UploadDataController(IUploadDataService uploadDataService)
        {
            _uploadDataService = uploadDataService;
        }

        [HttpPost]
        [Route("GenerateAssistantData")]
        public IActionResult GenerateAssistantData(IFormFile formFile)
        {
            if (formFile == null)
                throw new Exception("Não é possível importar a planilha sem ter uma planilha.");

            try
            {
                _uploadDataService.GenerateAssistantData(formFile);
                return Ok("Arquivo gerado com sucesso 'data_assistant.txt'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}