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

        [HttpGet]
        [Route("GetAnalyticsData")]
        public IActionResult GetAnalyticsData()
        {
            var returnMessage = _OpenAIService.GetAnalyticsData();
            return Ok(returnMessage);
        }

        [HttpPost]
        [Route("SendMessage")]
        public IActionResult SendMessage(string message)
        {
            var returnMessage = _OpenAIService.SendMessageToOpenAI(message);
            return Ok(returnMessage);
        }
    }
}