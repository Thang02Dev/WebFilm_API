using Microsoft.EntityFrameworkCore;
using WebFilm_API.Commons;
using WebFilm_API.DB;
using WebFilm_API.Models;
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

        public async Task<bool> ChangedTopView(int id)
        {
            var movie = await _dbContext.Movies.FirstAsync(x => x.Id == id);
            movie.Top_View = !movie.Top_View;
            await _dbContext.SaveChangesAsync();
#pragma warning disable CS8629 // Nullable value type may be null.
            return (bool)movie.Top_View;
#pragma warning restore CS8629 // Nullable value type may be null.
        }
        public async Task<bool> ChangedHot(int id)
        {
            var movie = await _dbContext.Movies.FirstAsync(x => x.Id == id);
            movie.Hot = !movie.Hot;
            await _dbContext.SaveChangesAsync();
            #pragma warning disable CS8629 // Nullable value type may be null.
            return (bool) movie.Hot;
            #pragma warning restore CS8629 // Nullable value type may be null.
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

            foreach (var item in model.GenreId.ToList())
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

        public async Task<bool> Delete(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return false;
            foreach (var mg in await _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).ToListAsync())
            {
                _dbContext.MovieGenres.Remove(mg);
            }
            foreach (var ep in await _dbContext.Episodes.Where(x=>x.MovieId==movie.Id).ToListAsync())
            {
                _dbContext.Episodes.Remove(ep);
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<MovieViewModel>> GetAll()
        {
            var query = from movie in _dbContext.Movies
                        join category in _dbContext.Categories 
                            on movie.CategoryId equals category.Id into joinCategories
                            from joinCategory in joinCategories.DefaultIfEmpty()
                        join country in _dbContext.Countries
                            on movie.CountryId equals country.Id into joinCountries
                            from joinCountry in joinCountries.DefaultIfEmpty()
                        orderby movie.Id descending
                        select new MovieViewModel
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Description = movie.Description,
                            Position = movie.Position,
                            Slug = movie.Slug,
                            Status = movie.Status,
                            CategoryId = movie.CategoryId,
                            CountryId = movie.CountryId,
                            Created_Date = movie.Created_Date,
                            Updated_Date = movie.Updated_Date,
                            Duration_Minutes = movie.Duration_Minutes,
                            Episode_Number = movie.Episode_Number,
                            Hot = movie.Hot,
                            Image = movie.Image,
                            Name_Eng = movie.Name_Eng,
                            Resolution = movie.Resolution,
                            Subtitle = movie.Subtitle,
                            Top_View = movie.Top_View,
                            Trailer = movie.Trailer,
                            Year_Release = movie.Year_Release,
                            Tags = movie.Tags,
                            GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x=>x.GenreId).ToList(),
                            CountryName = joinCountry.Name,
                            CategoryName = joinCategory.Name,
                            CountEpisodes = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).ToList().Count,
                        };
            return await query.ToListAsync();
        }

        public async Task<MovieViewModel?> GetById(int id)
        {
            var query = from movie in _dbContext.Movies
                        join category in _dbContext.Categories
                            on movie.CategoryId equals category.Id into joinCategories
                        from joinCategory in joinCategories.DefaultIfEmpty()
                        join country in _dbContext.Countries
                            on movie.CountryId equals country.Id into joinCountries
                        from joinCountry in joinCountries.DefaultIfEmpty()
                        where movie.Id == id
                        select new MovieViewModel
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Description = movie.Description,
                            Position = movie.Position,
                            Slug = movie.Slug,
                            Status = movie.Status,
                            CategoryId = movie.CategoryId,
                            CountryId = movie.CountryId,
                            Created_Date = movie.Created_Date,
                            Updated_Date = movie.Updated_Date,
                            Duration_Minutes = movie.Duration_Minutes,
                            Episode_Number = movie.Episode_Number,
                            Hot = movie.Hot,
                            Image = movie.Image,
                            Name_Eng = movie.Name_Eng,
                            Resolution = movie.Resolution,
                            Subtitle = movie.Subtitle,
                            Top_View = movie.Top_View,
                            Trailer = movie.Trailer,
                            Year_Release = movie.Year_Release,
                            Tags = movie.Tags,
                            GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x => x.GenreId).ToList(),
                            CountryName = joinCountry.Name,
                            CategoryName = joinCategory.Name,
                        };
            var _movie = await query.FirstOrDefaultAsync();
            if (_movie == null) return null;
            foreach (var item in _movie.GenreId)
            {
                var genreName = await _dbContext.Genres.FirstAsync(x => x.Id == item);
                _movie.GenreName.Add(genreName.Name);
            }
            return _movie;
        }

        public async Task<MoviePagin?> Pagination(int currentPage)
        {
            var pageResults = 20f;
            var pageCount = Math.Ceiling(_dbContext.Movies.Count() / pageResults);

            var movies = await GetAll();

            var result = movies.Skip((currentPage - 1) * (int)pageResults).Take((int)pageResults).ToList();

            if (result == null) return null;

            var moviePagin = new MoviePagin
            {
                MovieViewModels = result,
                CurrentPage = currentPage,
                PageCount = (int)pageCount
            };
            return moviePagin;
        }

        public async Task<MovieViewModel?> Update(int id, MovieViewModel model)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return null;
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Position = model.Position;
            movie.Slug = ConvertDatas.ConvertToSlug(model.Title);
            movie.Status = true;
            movie.CategoryId = model.CategoryId;
            movie.CountryId = model.CountryId;
            movie.Updated_Date = DateTime.Now;
            movie.Duration_Minutes = model.Duration_Minutes;
            movie.Episode_Number = model.Episode_Number;
            movie.Image = model.Image;
            movie.Name_Eng = model.Name_Eng;
            movie.Resolution = model.Resolution;
            movie.Subtitle = model.Subtitle;
            movie.Trailer = model.Trailer;
            movie.Year_Release = model.Year_Release;
            movie.Tags = model.Tags;
            movie.Top_View = model.Top_View;
            movie.Hot = model.Hot;
            await _dbContext.SaveChangesAsync();

            var rs = _dbContext.MovieGenres.Where(x => x.MovieId == id).ToList();
            foreach(var item in rs)
            {
                _dbContext.MovieGenres.Remove(item);
                await _dbContext.SaveChangesAsync();
            }

            foreach (var item in model.GenreId.ToList())
            {
                var moviegenre = new MovieGenre
                {
                    MovieId = id,
                    GenreId = item,
                };
                await _dbContext.MovieGenres.AddAsync(moviegenre);
                await _dbContext.SaveChangesAsync();
            }
            return model;
        }

        public async Task<List<MovieViewModel>> GetByStatus()
        {
            var query = from movie in _dbContext.Movies
                        where movie.Status == true
                        orderby movie.Title 
                        select new MovieViewModel
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                        };
            return await query.ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _dbContext.Movies.CountAsync();
        }

        public async Task<List<MovieViewModel>> GetByCategorySlug(string cateSlug)
        {
            int take=8;
            if (cateSlug == "phim-bo") take = 20;

            var query = (from movie in _dbContext.Movies
                        join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                        where movie.Status == true && cate.Slug == cateSlug.ToLower()
                        orderby movie.Position
                        select new MovieViewModel
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Image = movie.Image,
                            Episode_Number = movie.Episode_Number,
                            CountEpisodes = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).ToList().Count,
                            Subtitle = movie.Subtitle,
                            Slug = movie.Slug,
                            Position = movie.Position,
                            CategoryName = cate.Name
                        }).Take(take);
            return await query.ToListAsync();
        }
        public async Task<List<MovieViewModel>> GetByGenreSlug(string genreSlug)
        {
            var query = (from moviegenre in _dbContext.MovieGenres
                         join movie in _dbContext.Movies on moviegenre.MovieId equals movie.Id
                         join genre in _dbContext.Genres on moviegenre.GenreId equals genre.Id
                         where movie.Status == true && genre.Slug == genreSlug
                         orderby movie.Position
                         select new MovieViewModel
                         {
                             Id = movie.Id,
                             Title = movie.Title,
                             Image = movie.Image,
                             Episode_Number = movie.Episode_Number,
                             CountEpisodes = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).ToList().Count,
                             Subtitle = movie.Subtitle,
                             Slug = movie.Slug,
                             Position = movie.Position,
                             GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x => x.GenreId).ToList(),
                         }).Take(8);
            return await query.ToListAsync();
        }
    }
}
