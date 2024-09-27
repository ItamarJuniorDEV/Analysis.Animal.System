using Analysis.Animal.System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Analysis.Animal.System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _OpenAIService;

        public OpenAIController(IOpenAIService OpenAIService)
        {
            _OpenAIService = OpenAIService;
        }

        [HttpPost]
        [Route("GenerateHtml")]
        public IActionResult GenerateHtml()
        {
            var html = _OpenAIService.GenerateHtml("Crie um roteiro de viagens para esse fim de semana e retorne em html e css com tudo num arquivo html estilizado, não coloque testo no retorno, apenas código, sem markdown (```html)");
            return Ok(html);
        }
    }
}