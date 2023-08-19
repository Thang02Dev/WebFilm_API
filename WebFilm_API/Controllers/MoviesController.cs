using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.CategoryServices;
using WebFilm_API.Services.MovieService;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]MovieViewModel model)
        {
            if (await _service.CheckName(model.Title)) return BadRequest("Tên danh mục đã tồn tại!");
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
    }
}
