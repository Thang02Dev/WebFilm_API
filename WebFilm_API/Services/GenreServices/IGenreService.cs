using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.GenreServices
{
    public interface IGenreService
    {
        Task<List<GenreViewModel>> GetAll();
        Task<GenreViewModel?> GetById(int id);
        Task<GenreViewModel?> Create(GenreViewModel model);
        Task<bool> Delete(int id);
        Task<GenreViewModel?> Update(int id, GenreViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
    }
}
