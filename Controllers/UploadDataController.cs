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
        [Route("UploadSupportSheet")]
        public IActionResult UploadSupportSheet(IFormFile formFile)
        {
            if (formFile == null)
                throw new Exception("Não é possível importar a planilha sem ter uma planilha.");

            var message = _uploadDataService.UploadSupportSheet(formFile);
            return Ok(message);
        }

        [HttpPost]
        [Route("UploadFarm")]
        public IActionResult UploadFarm(IFormFile formFile)
        {
            if (formFile == null)
                throw new Exception("Não é possível importar a planilha sem ter uma planilha.");

            var message = _uploadDataService.UploadFarm(formFile);
            return Ok(message);
        }
    }
}