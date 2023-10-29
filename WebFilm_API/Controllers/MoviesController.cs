using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.MovieServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("pagin-search/{page:int}")]
        public async Task<IActionResult> GetPaginSearch(int page,string value)
        {
            var rs = await _service.Pagination(page,value);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("pagin-filter-cate/{page:int}")]
        public async Task<IActionResult> GetPaginCate(int page, int cateId, int order, int genreId, int countryId, int year)
        {
            var rs = await _service.PaginFilterByCate(page, cateId, order, genreId, countryId, year);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("pagin-filter-genre/{page:int}")]
        public async Task<IActionResult> GetPaginGenre(int page, int order, int genreId, int countryId, int year)
        {
            var rs = await _service.PaginFilterByGenre(page, order, genreId, countryId, year);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("pagin-filter-country/{page:int}")]
        public async Task<IActionResult> GetPaginCountry(int page, int order, int genreId, int countryId, int year)
        {
            var rs = await _service.PaginFilterByCountry(page, order, genreId, countryId, year);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("pagin-filter-year/{page:int}")]
        public async Task<IActionResult> GetPaginYear(int page, int order, int genreId, int countryId, int year)
        {
            var rs = await _service.PaginFilterByYear(page, order, genreId, countryId, year);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rs = await _service.GetAll();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("search/{value}")]
        public async Task<IActionResult> Searching(string value)
        {
            var rs = await _service.Searching(value);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var rs = await _service.GetBySlug(slug);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-categoryslug")]
        public async Task<IActionResult> GetByCategorySlug(string cateSlug)
        {
            var rs = await _service.GetByCategorySlug(cateSlug);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-genreslug")]
        public async Task<IActionResult> GetByGenreSlug( string genreSlug= "hoat-hinh")
        {
            var rs = await _service.GetByGenreSlug(genreSlug);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-count")]
        public async Task<IActionResult> GetCount()
        {
            var rs = await _service.GetCount();
            return Ok(rs);
        }
        [HttpGet("get-by-hot")]
        public async Task<IActionResult> GetByHot()
        {
            var rs = await _service.GetByHot();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-status")]
        public async Task<IActionResult> GetByStatus()
        {
            var rs = await _service.GetByStatus();
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetPagin(int page)
        {
            var rs = await _service.Pagination(page);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-cateid/{page:int}")]
        public async Task<IActionResult> GetPaginCate(int page,int cateId)
        {
            var rs = await _service.PaginationByCate(page, cateId);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-genreid/{page:int}")]
        public async Task<IActionResult> GetPaginGenre(int page, int genreId)
        {
            var rs = await _service.PaginationByGenre(page, genreId);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-countryid/{page:int}")]
        public async Task<IActionResult> GetPaginCountry(int page, int countryId)
        {
            var rs = await _service.PaginationByCountry(page, countryId);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-year/{page:int}")]
        public async Task<IActionResult> GetPaginYear(int page, int year)
        {
            var rs = await _service.PaginationByYear(page, year);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rs = await _service.GetById(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]MovieViewModel model)
        {
            if (await _service.CheckName(model.Title)) return BadRequest("Tên phim đã tồn tại!");
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id,[FromForm] MovieViewModel model)
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
        [HttpPost("changed-position")]
        public async Task<IActionResult> ChangedPosition(int id, int newPosition)
        {
            var rs = await _service.ChangedPosition(id, newPosition);
            return Ok(rs);
        }
        [HttpPost("changed-hot")]
        public async Task<IActionResult> ChangedHot(int id)
        {
            var rs = await _service.ChangedHot(id);
            return Ok(rs);
        }
        [HttpPost("changed-topview")]
        public async Task<IActionResult> ChangedTopView(int id)
        {
            var rs = await _service.ChangedTopView(id);
            return Ok(rs);
        }
    }
}
