using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebFilm_API.Services.GenreServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;
        public GenresController(IGenreService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _service.GetAll();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-count")]
        [Authorize]
        public async Task<IActionResult> GetCount()
        {
            var rs = await _service.GetCount();
            return Ok(rs);
        }
        [HttpGet("get-by-slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var rs = await _service.GetBySlug(slug);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus()
        {
            var rs = await _service.GetByStatusTrue();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var rs = await _service.GetById(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(GenreViewModel model)
        {
            if (await _service.CheckName(model.Name)) return BadRequest("Tên thể loại đã tồn tại!");
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(int id, GenreViewModel model)
        {
            var rs = await _service.Update(id, model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _service.Delete(id);
            if (!rs) return BadRequest();
            return Ok(rs);
        }
        [HttpPost("changed-status")]
        [Authorize]
        public async Task<IActionResult> ChangedStatus(int id)
        {
            var rs = await _service.ChangedStatus(id);
            return Ok(rs);
        }
    }
}
