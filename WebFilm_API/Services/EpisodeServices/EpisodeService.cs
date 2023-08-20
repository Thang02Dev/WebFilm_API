using Microsoft.EntityFrameworkCore;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.EpisodeServices
{
    public class EpisodeService : IEpisodeService
    {
        private readonly MyDbContext _dbContext;
        public EpisodeService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EpisodeViewModel?> Create(EpisodeViewModel model)
        {
            if (model == null) return null;
            var ep = new Episode
            {
                
                MovieId = model.MovieId,
                Link = model.Link,
                Episode_Number = model.Episode_Number,
                LinkServerId = model.LinkServerId,
            };
            await _dbContext.Episodes.AddAsync(ep);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var ep = await _dbContext.Episodes.FirstOrDefaultAsync(x => x.Id == id);
            if (ep == null) return false;
            _dbContext.Episodes.Remove(ep);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<EpisodeViewModel>> GetAll()
        {
            #pragma warning disable CS8629 // Nullable value type may be null.
            var query = from ep in _dbContext.Episodes
                        orderby ep.Id descending
                        select new EpisodeViewModel
                        {
                            Id = ep.Id,
                            Link = ep.Link,
                            Episode_Number = ep.Episode_Number,
                            MovieId = ep.MovieId,
                            LinkServerId =(int) ep.LinkServerId,
                        };
            #pragma warning restore CS8629 // Nullable value type may be null.
            return await query.ToListAsync();
        }

        public async Task<EpisodeViewModel?> GetById(int id)
        {
            #pragma warning disable CS8629 // Nullable value type may be null.
            var query = from ep in _dbContext.Episodes
                        where ep.Id == id
                        select new EpisodeViewModel
                        {
                            Id = ep.Id,
                            Link = ep.Link,
                            Episode_Number = ep.Episode_Number,
                            MovieId = ep.MovieId,
                            LinkServerId = (int)ep.LinkServerId,
                        };
            #pragma warning restore CS8629 // Nullable value type may be null.
            return await query.FirstOrDefaultAsync();
        }

        public async Task<EpisodeViewModel?> Update(int id, EpisodeViewModel model)
        {
            var ep = await _dbContext.Episodes.FirstOrDefaultAsync(x => x.Id == id);
            if (ep == null) return null;
            ep.LinkServerId = model.LinkServerId;
            ep.MovieId = model.MovieId;
            ep.Link = model.Link;
            ep.Episode_Number = model.Episode_Number;
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
