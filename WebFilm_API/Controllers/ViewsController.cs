using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.MovieServices;
using WebFilm_API.Services.ViewServices;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : ControllerBase
    {
        private readonly IViewService _service;

        public ViewsController(IViewService service) 
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _service.GetAll();
            return Ok(rs);
        }
        [HttpGet("page/{page:int}")]
        public async Task<IActionResult> GetPagin(int page)
        {
            var rs = await _service.Pagination(page);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-view-all/{page:int}")]
        public async Task<IActionResult> GetCountPagin(int page)
        {
            var rs = await _service.PaginationCountView(page);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpPost("created-view/{movieId:int}")]
        public async Task<IActionResult> CreatedView(int movieId)
        {
            var rs = await _service.CreatedView(movieId, HttpContext);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpGet("get-view-all")]
        public async Task<IActionResult> GetCountAll()
        {
            var rs = await _service.GetCountAll();
            return Ok(rs);
        }
        [HttpGet("get-view-day")]
        public async Task<IActionResult> GetCountByDay()
        {
            var rs = await _service.GetCountByDay();
            return Ok(rs);
        }
        [HttpGet("get-view-week")]
        public async Task<IActionResult> GetCountByWeek()
        {
            var rs = await _service.GetCountByWeek();
            return Ok(rs);
        }
        [HttpGet("get-view-month")]
        public async Task<IActionResult> GetCountByMonth()
        {
            var rs = await _service.GetCountByMonth();
            return Ok(rs);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _service.Delete(id);
            if (!rs) return BadRequest();
            return Ok("Xóa thông tin hoàn tất");
        }

    }
}
