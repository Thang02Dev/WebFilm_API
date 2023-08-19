using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.CountryServices
{
    public interface ICountryService
    {
        Task<List<CountryViewModel>> GetAll();
        Task<CountryViewModel?> GetById(int id);
        Task<CountryViewModel?> Create(CountryViewModel model);
        Task<bool> Delete(int id);
        Task<CountryViewModel?> Update(int id, CountryViewModel model);
        Task<bool> CheckName(string name);
        Task<bool> ChangedStatus(int id);
    }
}
