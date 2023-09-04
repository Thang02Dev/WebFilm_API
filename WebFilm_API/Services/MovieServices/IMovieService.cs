using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.MovieServices
{
    public interface IMovieService
    {
        Task<List<MovieViewModel>> GetAll();
        Task<List<MovieViewModel>> GetByStatus();
        Task<List<MovieViewModel>> GetByCategorySlug(string cateSlug);
        Task<List<MovieViewModel>> GetByGenreSlug(string genreSlug);
        Task<int> GetCount();
        Task<MovieViewModel?> GetById(int id);
        Task<MovieViewModel?> Create(MovieViewModel model);
        Task<bool> Delete(int id);
        Task<MovieViewModel?> Update(int id, MovieViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
        Task<bool> ChangedHot(int id);
        Task<int> ChangedPosition(int id, int newPosition);
        Task<bool> ChangedTopView(int id);
        Task<MoviePagin?> Pagination(int currentPage);
    }
}
