using Microsoft.AspNetCore.Mvc.RazorPages;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll();
        Task<CategoryViewModel?> GetById(int id);
        Task<CategoryViewModel?> Create(CategoryViewModel model);
        Task<bool> Delete(int id);
        Task<CategoryViewModel?> Update(int id, CategoryViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
        Task<int> ChangedPosition(int id,int newPosition);
    }
}
