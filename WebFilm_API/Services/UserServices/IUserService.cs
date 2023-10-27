using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewModel>?> GetAll();
        Task<UserViewModel?> GetById(int id);
        Task<UserViewModel?> Update(int id, UserViewModel model);
        Task<bool> Create(UserViewModel model);
        Task<bool> Delete(int id);
        Task<bool> Login(LoginViewModel model);
        Task<bool> ChangedStatus(int id);
        string GenerateToken(LoginViewModel model);
    }
}
