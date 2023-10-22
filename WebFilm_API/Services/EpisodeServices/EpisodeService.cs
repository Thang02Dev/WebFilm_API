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

            var movie = await _dbContext.Movies.SingleAsync(x => x.Id == ep.MovieId);
            movie.Updated_Date = DateTime.Now;
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

        public async Task<EpisodeViewModel> GetGroupByEpisodeNumber(int number,int movieId,int serverId)
        {
            var groupedEpisodes = await _dbContext.Episodes.OrderBy(x => x.Episode_Number)
                .Where(x => x.Episode_Number == number && x.MovieId==movieId && x.LinkServerId==serverId)
                .GroupBy(e => e.MovieId)
                .ToListAsync();

            List<EpisodeViewModel> result = new List<EpisodeViewModel>();

            foreach (var group in groupedEpisodes)
            {
                foreach (var episode in group)
                {
#pragma warning disable CS8629 // Nullable value type may be null.
                    result.Add(new EpisodeViewModel
                    {
                        Id = episode.Id,
                        MovieId = episode.MovieId,
                        Link = episode.Link,
                        Episode_Number = episode.Episode_Number,
                        LinkServerId = (int)episode.LinkServerId,
                        ServerName = _dbContext.LinkServers.Single(x => x.Id == episode.LinkServerId).Name,
                    });
#pragma warning restore CS8629 // Nullable value type may be null.
                }
            }

            return result.Single();
        }
        public async Task<List<EpisodeViewModel>> GetServer(int number, int movieId)
        {
            var groupedEpisodes = await _dbContext.Episodes
                .Where(x => x.Episode_Number == number && x.MovieId == movieId)
                .GroupBy(e => e.LinkServerId)
                .ToListAsync();

            List<EpisodeViewModel> result = new List<EpisodeViewModel>();

            foreach (var group in groupedEpisodes)
            {
                foreach (var episode in group)
                {
#pragma warning disable CS8629 // Nullable value type may be null.
                    result.Add(new EpisodeViewModel
                    {
                        Id = episode.Id,
                        MovieId = episode.MovieId,
                        Link = episode.Link,
                        Episode_Number = episode.Episode_Number,
                        LinkServerId = (int)episode.LinkServerId,
                        ServerName = _dbContext.LinkServers.Single(x => x.Id == episode.LinkServerId).Name,
                    });
#pragma warning restore CS8629 // Nullable value type may be null.
                }
            }

            return result.OrderBy(x=>x.ServerName).ToList();
        }

        public async Task<List<EpisodeViewModel>> GetGroupByMovieId(int movie_id)
        {
            var groupedEpisodes = await _dbContext.Episodes.OrderBy(x=>x.Episode_Number)
                .Where(x=>x.MovieId==movie_id)
                .GroupBy(e => e.MovieId)
                .ToListAsync();

            List<EpisodeViewModel> result = new List<EpisodeViewModel>();

            foreach (var group in groupedEpisodes)
            {
                foreach (var episode in group)
                {
#pragma warning disable CS8629 // Nullable value type may be null.
                    result.Add(new EpisodeViewModel
                    {
                        Id = episode.Id,
                        MovieId = episode.MovieId,
                        Link = episode.Link,
                        Episode_Number = episode.Episode_Number,
                        LinkServerId = (int)episode.LinkServerId,
                        ServerName = _dbContext.LinkServers.Single(x=>x.Id==episode.LinkServerId).Name,
                    });
#pragma warning restore CS8629 // Nullable value type may be null.
                }
            }

            return result;
        }
        public async Task<List<EpisodeViewModel>> GetEpisodes(int movie_id)
        {
            var distinctEpisodeNumbers = await _dbContext.Episodes
                .Where(x => x.MovieId == movie_id)
                .Select(x => x.Episode_Number)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            List<EpisodeViewModel> result = new List<EpisodeViewModel>();

            foreach (var episodeNumber in distinctEpisodeNumbers)
            {
                // Lấy một episode từ mỗi episode_number
                var episode = await _dbContext.Episodes
                    .Where(x => x.MovieId == movie_id && x.Episode_Number == episodeNumber)
                    .FirstOrDefaultAsync();

                if (episode != null)
                {
                    result.Add(new EpisodeViewModel
                    {
                        Id = episode.Id,
                        MovieId = episode.MovieId,
                        Link = episode.Link,
                        Episode_Number = episode.Episode_Number,
                        LinkServerId = episode.LinkServerId ?? 0,
                        ServerName = _dbContext.LinkServers
                            .FirstOrDefault(x => x.Id == episode.LinkServerId)?.Name ?? string.Empty
                    });
                }
            }

            return result;
        }

        public async Task<EpisodePagin?> Pagination(int movieId,int currentPage)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_dbContext.Episodes.Where(x=>x.MovieId==movieId).Count() / pageResults);

            var episodes = await GetGroupByMovieId(movieId);

            var result = episodes.Skip((currentPage - 1) * (int)pageResults).Take((int)pageResults).ToList();

            if (result == null) return null;

            var episodePagin = new EpisodePagin
            {
                EpisodeViewModels = result,
                CurrentPage = currentPage,
                PageCount = (int)pageCount
            };
            return episodePagin;
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
