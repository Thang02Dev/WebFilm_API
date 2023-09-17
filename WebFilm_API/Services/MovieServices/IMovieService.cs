using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.MovieServices
{
    public interface IMovieService
    {
        Task<List<MovieViewModel>> GetAll();
        Task<List<MovieViewModel>> GetByStatus();
        Task<List<MovieViewModel>> GetByHot();
        Task<List<MovieViewModel>> GetByCategorySlug(string cateSlug);

        Task<List<MovieViewModel>> GetByCategoryId(int cateId);
        Task<List<MovieViewModel>> GetByGenreId(int genreId);
        Task<List<MovieViewModel>> GetByCountryId(int countryId);
        Task<List<MovieViewModel>> GetByYear(int year);

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
        Task<MoviePagin?> PaginationByCate(int currentPage, int cateId);
        Task<MoviePagin?> PaginationByGenre(int currentPage, int genreId);
        Task<MoviePagin?> PaginationByCountry(int currentPage, int countryId);
        Task<MoviePagin?> PaginationByYear(int currentPage, int year);
        Task<List<MovieViewModel>?> GetFilterByCate(int cateId,int order,int genreId, int countryId, int year);
        Task<List<MovieViewModel>?> GetFilter(int order,int genreId, int countryId, int year);
        Task<MoviePagin?> PaginFilterByCate(int currentPage, int cateId, int order, int genreId, int countryId, int year);
        Task<MoviePagin?> PaginFilterByGenre(int currentPage, int order, int genreId, int countryId, int year);
        Task<MoviePagin?> PaginFilterByCountry(int currentPage, int order, int genreId, int countryId, int year);
        Task<MoviePagin?> PaginFilterByYear(int currentPage, int order, int genreId, int countryId, int year);


    }
}
