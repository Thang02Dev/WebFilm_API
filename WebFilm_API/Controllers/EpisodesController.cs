using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm_API.Services.EpisodeServices;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodeService _service;
        public EpisodesController(IEpisodeService service)
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
        [HttpGet("{page:int}")]
        public async Task<IActionResult> GetPagin(int id,int page)
        {
            var rs = await _service.Pagination(id,page);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-movieid/{id:int}")]
        public async Task<IActionResult> GetGroupByMovieId(int id)
        {
            var rs = await _service.GetGroupByMovieId(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-episodes/{id:int}")]
        public async Task<IActionResult> GetEpidoes(int id)
        {
            var rs = await _service.GetEpisodes(id);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-by-episode-number/{id:int}")]
        public async Task<IActionResult> GetGroupByEpisodeNumber(int id,int movieId, int serverId)
        {
            var rs = await _service.GetGroupByEpisodeNumber(id,movieId,serverId);
            if (rs == null) return NotFound();
            return Ok(rs);
        }
        [HttpGet("get-server/{id:int}")]
        public async Task<IActionResult> GetServer(int id, int movieId)
        {
            var rs = await _service.GetServer(id, movieId);
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
        public async Task<IActionResult> Create(EpisodeViewModel model)
        {
            var rs = await _service.Create(model);
            if (rs == null) return BadRequest();
            return Ok(rs);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, EpisodeViewModel model)
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
    }
}
