using CRM.Service;
using Microsoft.AspNetCore.Mvc;

namespace CRM.ChatGpt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGptController : ControllerBase
    {
        private readonly IChatGptService _chatGptService;

        public ChatGptController(IChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
        }

        [HttpPost("suggest-content")]
        public async Task<IActionResult> SuggestContent([FromBody] string contentBody = "")
        {
            try
            {
                string generatedText = await _chatGptService.GenerateSuggestedContent(contentBody);
                return Ok(generatedText);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}