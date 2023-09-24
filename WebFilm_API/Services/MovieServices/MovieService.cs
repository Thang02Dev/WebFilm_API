using Microsoft.EntityFrameworkCore;
using System.Text;
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
                Director = model.Director,
                Performer = model.Performer
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
                            Director = movie.Director,
                            Performer = movie.Performer,
                            GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x=>x.GenreId).ToList(),
                            CountryName = joinCountry.Name,
                            CategoryName = joinCategory.Name,
                            CountEpisodes = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).ToList().Count,
                        };
            return await query.ToListAsync();
        }
        public async Task<MovieViewModel?> GetById(int id)
        {
            var epiCount = 0;
            var epiNew = 0;
            var episodes = await _dbContext.Episodes.Where(x => x.MovieId == id).ToListAsync();
            if (episodes.Count() != 0)
            {
                epiCount = episodes.Count();
                epiNew = episodes.Max(x => x.Episode_Number);
            }          
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
                            Director = movie.Director,
                            Performer = movie.Performer,
                            EpisodeNew = epiNew
                        };
            var _movie = await query.FirstOrDefaultAsync();
            if (_movie == null) return null;
            foreach (var item in _movie.GenreId)
            {
                var genreName = await _dbContext.Genres.FirstAsync(x => x.Id == item);
                _movie.GenreName.Add(genreName.Name);
            }
            if (_movie.Episode_Number == epiCount)
            {
                _movie.Condition = "Hoàn tất";
                _movie.EpisodeStatus = $"Full {epiCount}/{epiCount} Vietsub";
            }
            else
            {
                _movie.Condition = "Phim đang chiếu";
                _movie.EpisodeStatus = $"Tập {epiNew}";
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
        public async Task<MoviePagin?> Pagination(int currentPage,string valueSearch)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(Searching(valueSearch).Result.Count() / pageResults);

            var movies = await Searching(valueSearch);

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
            movie.Performer = model.Performer;
            movie.Director = model.Director;
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
                            CategoryName = cate.Name,
                            EpisodeNew = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).Max(x => x.Episode_Number),
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
                             EpisodeNew = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).Max(x => x.Episode_Number),

                         }).Take(8);
            return await query.ToListAsync();
        }
        public async Task<List<MovieViewModel>> GetByHot()
        {
            var query = (from moviegenre in _dbContext.MovieGenres
                         join movie in _dbContext.Movies on moviegenre.MovieId equals movie.Id
                         join genre in _dbContext.Genres on moviegenre.GenreId equals genre.Id
                         where movie.Status == true && movie.Hot == true
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
                             Updated_Date = movie.Updated_Date,
                             GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x => x.GenreId).ToList(),
                             EpisodeNew = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).Max(x => x.Episode_Number),
                         }).GroupBy(m => m.Id)
                            .Select(group => group.First()).Take(15);
            var result = await query.ToListAsync();
            result = result.OrderBy(m => m.Position).ToList();
            return result;

        }
        public async Task<MoviePagin?> PaginationByCate(int currentPage, int cateId)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x=>x.CategoryId==cateId).Count() / pageResults);

            var movies = await GetByCategoryId(cateId);

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

        public async Task<List<MovieViewModel>> GetByCategoryId(int cateId)
        {
            var category = await _dbContext.Categories.FirstAsync(x => x.Id == cateId);
            if(category.Slug == "phim-le")
            {
                var query = from movie in _dbContext.Movies
                            join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                            where movie.Status == true && cate.Id == cateId || cate.Slug == "phim-chieu-rap"
                            orderby movie.Updated_Date descending
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
                                CategoryName = cate.Name,
                                Updated_Date = movie.Updated_Date,
                                EpisodeNew = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).Max(x => x.Episode_Number),

                            };
                return await query.ToListAsync();
            }
            else
            {

                var query = from movie in _dbContext.Movies
                            join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                            where movie.Status == true && cate.Id == cateId
                            orderby movie.Updated_Date descending
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
                                CategoryName = cate.Name,
                                Updated_Date = movie.Updated_Date,
                                EpisodeNew = _dbContext.Episodes.Where(x => x.MovieId == movie.Id).Max(x => x.Episode_Number),

                            };
                return await query.ToListAsync();
            }
        }
        public async Task<List<MovieViewModel>> GetByGenreId(int genreId)
        {
            var query = (from moviegenre in _dbContext.MovieGenres
                         join movie in _dbContext.Movies on moviegenre.MovieId equals movie.Id
                         join genre in _dbContext.Genres on moviegenre.GenreId equals genre.Id
                         where movie.Status == true && genre.Id == genreId
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
                             Updated_Date = movie.Updated_Date,
                             GenreId = _dbContext.MovieGenres.Where(x => x.MovieId == movie.Id).Select(x => x.GenreId).ToList(),
                         }).GroupBy(m => m.Id)
                            .Select(group => group.First()).Take(15);
            var result = await query.ToListAsync();
            result = result.OrderByDescending(m => m.Updated_Date).ToList();
            return result;
        }
        public async Task<List<MovieViewModel>?> GetFilterByCate(int cateId,int order, int genreId, int countryId, int year)
        {
            
            if(genreId != 0 && countryId != 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if(order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if(order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
                
            }
            else if (genreId != 0 && countryId != 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query =(from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.CountryId == countryId
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                                                .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId != 0 && countryId == 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                   && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                     && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId != 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId != 0 && countryId == 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
                                    
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId && mg.GenreId == genreId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId != 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
                                    && movie.CountryId == countryId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId == 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId && movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId &&  movie.Year_Release == year.ToString()
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId == 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                where movie.Status == true && cate.Id == cateId 
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
                                    CategoryName = cate.Name,
                                    Updated_Date = movie.Updated_Date,
                                }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && cate.Id == cateId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            return null;
        }

        public async Task<MoviePagin?> PaginFilterByCate(int currentPage, int cateId, int order, int genreId, int countryId, int year)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x => x.CategoryId == cateId).Count() / pageResults);

            var movies = await GetFilterByCate(cateId, order, genreId, countryId, year);

            if (movies == null) return null;

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

        public async Task<MoviePagin?> PaginationByGenre(int currentPage, int genreId)
        {
            var pageResults = 24f;

            var pageCount = Math.Ceiling(_dbContext.MovieGenres.Where(x => x.GenreId == genreId).GroupBy(x=>x.MovieId).Count() / pageResults);

            var movies = await GetByGenreId(genreId);

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

        public async Task<List<MovieViewModel>?> GetFilter(int order, int genreId, int countryId, int year)
        {
            if (genreId != 0 && countryId != 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }

            }
            else if (genreId != 0 && countryId != 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                                                .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId != 0 && countryId == 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                    && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                     && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                      && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
                                     && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
                                    && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId != 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
                                     && movie.CountryId == countryId && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId != 0 && countryId == 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId

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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId

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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true  && mg.GenreId == genreId

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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && mg.GenreId == genreId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId != 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
                                     && movie.CountryId == countryId
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId == 0 && year != 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true && movie.Year_Release == year.ToString()
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            else if (genreId == 0 && countryId == 0 && year == 0)
            {
                if (order == 1)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Created_Date).ToList();
                    return result;
                }
                else if (order == 2)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Year_Release).ToList();
                    return result;
                }
                else if (order == 3)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderBy(m => m.Title).ToList();
                    return result;
                }
                else if (order == 4)
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    return await query.ToListAsync();
                }
                else
                {
                    var query = (from mg in _dbContext.MovieGenres
                                 join movie in _dbContext.Movies on mg.MovieId equals movie.Id
                                 join cate in _dbContext.Categories on movie.CategoryId equals cate.Id
                                 where movie.Status == true 
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
                                     CategoryName = cate.Name,
                                     Updated_Date = movie.Updated_Date,
                                 }).GroupBy(m => m.Id)
                            .Select(group => group.First());
                    var result = await query.ToListAsync();
                    result = result.OrderByDescending(m => m.Updated_Date).ToList();
                    return result;
                }
            }
            return null;
        }

        public async Task<MoviePagin?> PaginFilterByGenre(int currentPage, int order, int genreId, int countryId, int year)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(_dbContext.MovieGenres.Where(x => x.GenreId == genreId).GroupBy(x => x.MovieId).Count() / pageResults);

            var movies = await GetFilter( order, genreId, countryId, year);

            if (movies == null) return null;

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

        public async Task<MoviePagin?> PaginationByCountry(int currentPage, int countryId)
        {
            var pageResults = 24f;

            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x => x.CountryId == countryId).Count() / pageResults);

            var movies = await GetByCountryId(countryId);

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

        public async Task<MoviePagin?> PaginFilterByCountry(int currentPage, int order, int genreId, int countryId, int year)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x => x.CountryId == countryId).Count() / pageResults);

            var movies = await GetFilter(order, genreId, countryId, year);

            if (movies == null) return null;

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

        public async Task<List<MovieViewModel>> GetByCountryId(int countryId)
        {
            var query = from movie in _dbContext.Movies
                        join country in _dbContext.Countries on movie.CountryId equals country.Id
                        where movie.Status == true && country.Id == countryId
                        orderby movie.Updated_Date descending
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
                            Updated_Date = movie.Updated_Date,
                            CountryId = movie.CountryId,
                            CountryName = country.Name,
                        };
            return await query.ToListAsync();
        }

        public async Task<MoviePagin?> PaginationByYear(int currentPage, int year)
        {
            var pageResults = 24f;

            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x => x.Year_Release == year.ToString()).Count() / pageResults);

            var movies = await GetByYear(year);

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

        public async Task<MoviePagin?> PaginFilterByYear(int currentPage, int order, int genreId, int countryId, int year)
        {
            var pageResults = 24f;
            var pageCount = Math.Ceiling(_dbContext.Movies.Where(x => x.Year_Release == year.ToString()).Count() / pageResults);

            var movies = await GetFilter(order, genreId, countryId, year);

            if (movies == null) return null;

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

        public async Task<List<MovieViewModel>> GetByYear(int year)
        {
            var query = from movie in _dbContext.Movies
                        where movie.Status == true && movie.Year_Release == year.ToString()
                        orderby movie.Updated_Date descending
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
                            Updated_Date = movie.Updated_Date,
                            Year_Release = movie.Year_Release,
                        };
            return await query.ToListAsync();
        }

        public async Task<MovieViewModel?> GetBySlug(string slug)
        {
            var query = from movie in _dbContext.Movies
                        join country in _dbContext.Countries on movie.CountryId equals country.Id
                        where movie.Slug == slug
                        select new MovieViewModel
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            CountryName = country.Name,
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<MovieViewModel>> Searching(string value)
        {
            var query = from movie in _dbContext.Movies
                        where movie.Status == true 
                                && movie.Title.ToLower().Contains(value.ToLower()) 
                                || movie.Slug.Contains(ConvertDatas.ConvertToSlug(value))
                                || movie.Name_Eng.ToLower().Contains(value.ToLower())
                        orderby movie.Updated_Date descending
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
                            Updated_Date = movie.Updated_Date,
                        };
            return await query.ToListAsync();
        }
    }
}
