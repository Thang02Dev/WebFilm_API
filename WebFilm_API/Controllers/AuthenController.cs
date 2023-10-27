using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.UserServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IUserService _service;

        public AuthenController(IUserService service) {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = await _service.Login(model);
                if (!result) return BadRequest("Email hoặc mật khẩu không đúng");
                string token = _service.GenerateToken(model);
                HttpContext.Response.Cookies.Append("Login_Token", token, new CookieOptions
                {
                    HttpOnly = true, // Đảm bảo cookie không thể truy cập bằng mã JavaScript
                    SameSite = SameSiteMode.Strict, // Cấu hình SameSite
                    Secure = true, // Đảm bảo cookie chỉ được gửi qua kết nối bảo mật HTTPS
                    Expires = DateTimeOffset.Now.AddDays(1)// Thời gian tồn tại của cookie
                });
                return Ok(new
                {
                    mess = "Đăng nhập thành công!",
                    token,
                });
            }
        }
    }
}
