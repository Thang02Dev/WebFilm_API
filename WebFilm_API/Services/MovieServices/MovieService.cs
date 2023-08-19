using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFilm_API.Commons;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.Services.MovieService;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.MovieServices
{
    public class MovieService : IMovieService
    {
        private readonly MyDbContext _dbContext;
        public MovieService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> ChangedPosition(int id, int newPosition)
        {
            var movie = await _dbContext.Movies.FirstAsync(x => x.Id == id);
            movie.Position = newPosition;
            await _dbContext.SaveChangesAsync();
            return (int)movie.Position;
        }

        public async Task<bool> ChangedStatus(int id)
        {
            var movie = await _dbContext.Movies.FirstAsync(x => x.Id == id);
            movie.Status = !movie.Status;
            await _dbContext.SaveChangesAsync();
            return movie.Status;
        }

        public async Task<bool> CheckName(string name)
        {
            var rs = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Title.ToLower() == name.ToLower());
            if (rs == null) return false;
            return true;
        }

        public async Task<MovieViewModel?> Create(MovieViewModel model)
        {
            if (model == null) return null;
            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                Position = model.Position,
                Slug = ConvertDatas.ConvertToSlug(model.Title),
                Status = true,
                CategoryId = model.CategoryId,
                CountryId = model.CountryId,
                Created_Date = DateTime.Now,
                Duration_Minutes = model.Duration_Minutes,
                Episode_Number = model.Episode_Number,
                Hot = model.Hot,
                Image = model.Image,
                Name_Eng = model.Name_Eng,
                Resolution = model.Resolution,
                Subtitle = model.Subtitle,
                Top_View = model.Top_View,
                Trailer = model.Trailer,
                Year_Release =model.Year_Release,
                Tags = model.Tags,               
            };
            await _dbContext.Movies.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            foreach (var item in model.GenreId)
            {
                var moviegenre = new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = item,
                };
                await _dbContext.MovieGenres.AddAsync(moviegenre);
                await _dbContext.SaveChangesAsync();
            }
            return model;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MovieViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<MovieViewModel?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MoviePagin?> Pagination(int currentPage)
        {
            throw new NotImplementedException();
        }

        public Task<MovieViewModel?> Update(int id, MovieViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
