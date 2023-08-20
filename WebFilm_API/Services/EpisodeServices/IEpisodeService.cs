using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.EpisodeServices
{
    public interface IEpisodeService
    {
        Task<List<EpisodeViewModel>> GetAll();
        Task<EpisodeViewModel?> GetById(int id);
        Task<EpisodeViewModel?> Create(EpisodeViewModel model);
        Task<bool> Delete(int id);
        Task<EpisodeViewModel?> Update(int id, EpisodeViewModel model);

    }
}
