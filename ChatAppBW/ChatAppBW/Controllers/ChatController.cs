using ChatAppBW.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatModels;
using ChatModels.Models;
using ChatModels.Dto;

namespace ChatAppBW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(ChatRepository chatRepository) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync() =>
            Ok(await chatRepository.GetAvailableUsersAsync());

        [HttpPost("individual")]
        public async Task<IActionResult> GetIndividualChatsAsync(RequestChatDto requestChatDto) =>
            Ok(await chatRepository.GetIndividualChatsAsync(requestChatDto));
    }
}
