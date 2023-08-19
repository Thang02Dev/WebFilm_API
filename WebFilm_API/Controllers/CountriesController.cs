using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.DB;
using WebFilm_API.Services.CountryServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _service;
        public CountriesController(ICountryService service)
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rs = await _service.GetById(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (await _service.CheckName(model.Name)) return BadRequest("Tên quốc gia đã tồn tại!");
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, CountryViewModel model)
        {
            var rs = await _service.Update(id, model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _service.Delete(id);
            if (!rs) return BadRequest();
            return Ok(rs);
        }
        [HttpPost("changed-status")]
        public async Task<IActionResult> ChangedStatus(int id)
        {
            var rs = await _service.ChangedStatus(id);
            return Ok(rs);
        }
    }
}
