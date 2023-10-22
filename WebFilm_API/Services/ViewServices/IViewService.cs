using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.ViewServices
{
    public interface IViewService
    {
        Task<List<ViewViewModel>> GetAll();
        Task<bool> Delete(int id);
        Task<ViewPagin?> Pagination(int currentPage);
        Task<ViewViewModel?> CreatedView(int movieId, HttpContext context); 
        Task<List<ViewViewModel>> GetCountByDay();
        Task<List<ViewViewModel>> GetCountByMonth();
        Task<List<ViewViewModel>> GetCountByWeek();
    }
}
