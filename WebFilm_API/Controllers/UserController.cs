using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebFilm_API.Hubs;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<UserHub> _hubContext;
        private static int userCount = 0; // Số lượng người dùng

        public UserController(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetUserCount()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUserCount", userCount);
            return Ok(userCount);
        }

        [HttpPost("increment")]
        public async Task<IActionResult> IncrementUserCount()
        {
            // Simulate an increase in user count (you can use your own logic)
            userCount++;

            // Gửi thông tin số lượng người dùng đến Hub SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveUserCount", userCount);

            return Ok(userCount);
        }

        [HttpPost("decrement")]
        public async Task<IActionResult> DecrementUserCount()
        {
            // Simulate a decrease in user count (you can use your own logic)
            if (userCount > 0)
            {
                userCount--;
            }

            // Gửi thông tin số lượng người dùng đến Hub SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveUserCount", userCount);

            return Ok(userCount);
        }

    }
}
