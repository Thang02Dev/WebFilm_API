using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using WebFilm_API.DB;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.ViewServices
{
    public class ViewService : IViewService
    {
        private readonly MyDbContext _dbcontext;

        public ViewService(MyDbContext dbContext) 
        {
            _dbcontext = dbContext;
        }
        public string GetRemoteHostIpAddress(HttpContext context)
        {
            //lấy địa IpAddress trong Connection của request client gửi lên
            IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;
            //biến chứa địa chỉ IP V4
            string ipv4 = "";
            //nếu có giá trị
            if (remoteIpAddress != null)
            {
                //nếu là IP v6 thì chuyển về IP V4
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                //gán địa chỉ IP v4 vào biết kết quả
                ipv4 = remoteIpAddress.ToString();
            }
            //trả về kết quả
            return ipv4;
        }
        public async Task<ViewViewModel?> CreatedView(int movieId, HttpContext context)
        {
            var movie = await _dbcontext.Movies.FindAsync(movieId);
            if (movie == null) return null;
            var view = new Models.View
            {
                MovieId = movie.Id,
                DateViewed = DateTime.Now,
                ViewerIP = GetRemoteHostIpAddress(context),
            };
            await _dbcontext.Views.AddAsync(view);
            await _dbcontext.SaveChangesAsync();
            var model = new ViewViewModel
            {
                MovieId = movie.Id,
                DateViewed = view.DateViewed,
                ViewerIP = view.ViewerIP
            };
            return model;
        }

        public async Task<List<ViewViewModel>> GetCountByDay()
        {
            var query = from v in _dbcontext.Views
                        join m in _dbcontext.Movies on v.MovieId equals m.Id
                        where v.DateViewed.Value.Date == DateTime.Now.Date
                        group new { v, m } by v.MovieId into grouped
                        orderby grouped.Count() descending
                        select new ViewViewModel
                        {
                            MovieId = grouped.Key,
                            Title = grouped.First().m.Title,
                            Name_Eng = grouped.First().m.Name_Eng,
                            Count = grouped.Count(),
                        };
            return await query.Take(10).ToListAsync();
        }

        public async Task<List<ViewViewModel>> GetCountByMonth()
        {
            var query = from v in _dbcontext.Views
                        join m in _dbcontext.Movies on v.MovieId equals m.Id
                        where v.DateViewed.Value.Month == DateTime.Now.Month && v.DateViewed.Value.Year == DateTime.Now.Year
                        group new { v, m } by v.MovieId into grouped
                        orderby grouped.Count() descending
                        select new ViewViewModel
                        {
                            MovieId = grouped.Key,
                            Title = grouped.First().m.Title,
                            Name_Eng = grouped.First().m.Name_Eng,
                            Count = grouped.Count(),
                        };
            return await query.Take(10).ToListAsync();
        }

        public async Task<List<ViewViewModel>> GetCountByWeek()
        {
            int currentYear = DateTime.Now.Year;
            int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

            var movieViews = await _dbcontext.Views.ToListAsync();

            var currentWeekViews = movieViews
                .Where(view => view.DateViewed.HasValue &&
                    view.DateViewed.Value.Year == currentYear &&
                    CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(view.DateViewed.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday) == currentWeek);

            var query = from v in currentWeekViews
                        join m in _dbcontext.Movies on v.MovieId equals m.Id
                        group new { v, m } by v.MovieId into grouped
                        orderby grouped.Count() descending
                        select new ViewViewModel
                        {
                            MovieId = grouped.Key,
                            Title = grouped.First().m.Title,
                            Name_Eng = grouped.First().m.Name_Eng,
                            Count = grouped.Count(),
                        };

            return query.Take(10).ToList();
        }

        public async Task<List<ViewViewModel>> GetAll()
        {
            var query = from v in _dbcontext.Views
                        join m in _dbcontext.Movies on v.MovieId equals m.Id
                        orderby v.DateViewed descending
                        select new ViewViewModel
                        {
                            Id = v.Id,
                            MovieId = v.MovieId,
                            Title = m.Title,
                            Name_Eng = m.Name_Eng,
                            DateViewed = v.DateViewed,
                            ViewerIP = v.ViewerIP
                        };
            return await query.ToListAsync();
        }
        public async Task<ViewPagin?> Pagination(int currentPage)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_dbcontext.Views.Count() / pageResults);

            var views = await GetAll();

            var result = views.Skip((currentPage - 1) * (int)pageResults).Take((int)pageResults).ToList();

            if (result == null) return null;

            var viewPagin = new ViewPagin
            {
                ViewViewModels = result,
                CurrentPage = currentPage,
                PageCount = (int)pageCount
            };
            return viewPagin;
        }
        public async Task<bool> Delete(int id)
        {
            var view = await _dbcontext.Views.FirstOrDefaultAsync(x => x.Id == id);
            if (view == null) return false;
            _dbcontext.Views.Remove(view);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ViewViewModel>> GetCountAll()
        {
            var query = from v in _dbcontext.Views
                        join m in _dbcontext.Movies on v.MovieId equals m.Id
                        group new { v, m } by v.MovieId into grouped
                        orderby grouped.Count() descending
                        select new ViewViewModel
                        {
                            MovieId = grouped.Key,
                            Title = grouped.First().m.Title,
                            Name_Eng = grouped.First().m.Name_Eng,
                            Count = grouped.Count(),
                        };
            return await query.ToListAsync();
        }
        public async Task<object?> PaginationCountView(int currentPage)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(GetCountAll().Result.Count / pageResults);

            var views = await GetCountAll();

            var result = views.Skip((currentPage - 1) * (int)pageResults).Take((int)pageResults).ToList();

            if (result == null) return null;

            var viewPagin = new
            {
                Data = result,
                CurrentPage = currentPage,
                PageCount = (int)pageCount
            };
            return viewPagin;
        }
    }
}
