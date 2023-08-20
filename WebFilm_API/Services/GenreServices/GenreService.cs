using Microsoft.EntityFrameworkCore;
using WebFilm_API.Commons;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.GenreServices
{
    public class GenreService : IGenreService
    {
        private readonly MyDbContext _dbContext;
        public GenreService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ChangedStatus(int id)
        {
            var genre = await _dbContext.Genres.FirstAsync(x => x.Id == id);
            genre.Status = !genre.Status;
            await _dbContext.SaveChangesAsync();
            return genre.Status;
        }

        public async Task<bool> CheckName(string name)
        {
            var rs = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (rs == null) return false;
            return true;
        }

        public async Task<GenreViewModel?> Create(GenreViewModel model)
        {
            if (model == null) return null;
            var genre = new Genre
            {
                
                Name = model.Name,
                Description = model.Description,
                Slug = ConvertDatas.ConvertToSlug(model.Name),
                Status = true,
            };
            await _dbContext.Genres.AddAsync(genre);
            await _dbContext.SaveChangesAsync();
            return model; 

        }

        public async Task<bool> Delete(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null) return false;
            foreach (var item in await _dbContext.MovieGenres.Where(x => x.GenreId == genre.Id).ToListAsync())
            {
                _dbContext.MovieGenres.Remove(item);
            }
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<GenreViewModel>> GetAll()
        {
            var query = from genre in _dbContext.Genres
                        orderby genre.Id descending
                        select new GenreViewModel
                        {
                            Id = genre.Id,
                            Name = genre.Name,
                            Description = genre.Description,
                            Slug = genre.Slug,
                            Status = genre.Status,
                        };
            return await query.ToListAsync();
        }

        public async Task<GenreViewModel?> GetById(int id)
        {
            var query = from genre in _dbContext.Genres
                        where genre.Id == id
                        select new GenreViewModel
                        {
                            Id = genre.Id,
                            Name = genre.Name,
                            Description = genre.Description,
                            Slug = genre.Slug,
                            Status = genre.Status,
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<GenreViewModel?> Update(int id, GenreViewModel model)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null) return null;
            genre.Name = model.Name;
            genre.Description = model.Description;
            genre.Slug = ConvertDatas.ConvertToSlug(model.Name);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
