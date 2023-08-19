using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.MovieService
{
    public interface IMovieService
    {
        Task<List<MovieViewModel>> GetAll();
        Task<MovieViewModel?> GetById(int id);
        Task<MovieViewModel?> Create(MovieViewModel model);
        Task<bool> Delete(int id);
        Task<MovieViewModel?> Update(int id, MovieViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
        Task<int> ChangedPosition(int id, int newPosition);
        Task<MoviePagin?> Pagination(int currentPage);
    }
}
