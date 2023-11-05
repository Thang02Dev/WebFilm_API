using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebFilm_API.Hubs;
using WebFilm_API.Services.UserServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IUserService _service;
        private static int userCount = 0; // Số lượng người dùng

        public UsersController(IHubContext<UserHub> hubContext, IUserService service)
        {
            _hubContext = hubContext;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _service.GetAll();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var rs = await _service.GetById(id);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPost("changed-status")]
        public async Task<IActionResult> ChangedStatus(int id)
        {
            var rs = await _service.ChangedStatus(id);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            var rs = await _service.Create(model);
            if (!rs) return BadRequest("Email đã tồn tại!");
            return Ok("Tạo mới người dùng thành công!");
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserViewModel model)
        {
            var rs = await _service.Update(id, model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _service.Delete(id);
            if (!rs) return BadRequest();
            return Ok(rs);
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
