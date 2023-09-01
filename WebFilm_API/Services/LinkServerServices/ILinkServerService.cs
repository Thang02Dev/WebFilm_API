using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.LinkServerServices
{
    public interface ILinkServerService
    {
        Task<List<LinkServerViewModel>> GetAll();
        Task<List<LinkServerViewModel>> GetByStatus();
        Task<LinkServerViewModel?> GetById(int id);
        Task<LinkServerViewModel?> Create(LinkServerViewModel model);
        Task<bool> Delete(int id);
        Task<LinkServerViewModel?> Update(int id, LinkServerViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
    }
}
