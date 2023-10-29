using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.CategoryServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var rs = await _service.GetAll();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-count")]
        public async Task<IActionResult> GetCount()
        {
            var rs = await _service.GetCount();
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
        public async Task<IActionResult> GetById(int id) 
        {
            var rs = await _service.GetById(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-slug/{slug}")]
        public async Task<IActionResult> GetById(string slug)
        {
            var rs = await _service.GetBySlug(slug);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if(await _service.CheckName(model.Name)) return BadRequest("Tên danh mục đã tồn tại!");
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, CategoryViewModel model)
        {
            var rs = await _service.Update(id, model);
            if(rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _service.Delete(id);
            if(!rs) return BadRequest();
            return Ok(rs);
        }
        [HttpPost("changed-status")]
        public async Task<IActionResult> ChangedStatus(int id)
        {
            var rs = await _service.ChangedStatus(id);
            return Ok(rs);
        }
        [HttpPost("changed-position")]
        public async Task<IActionResult> ChangedPosition(int id, int newPosition)
        {
            var rs = await _service.ChangedPosition(id, newPosition);
            return Ok(rs);
        }
    }
}
