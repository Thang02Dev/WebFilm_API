using Azure.Core.GeoJson;
using Microsoft.EntityFrameworkCore;
using WebFilm_API.Commons;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.CountryServices
{
    public class CountryService : ICountryService
    {
        private readonly MyDbContext _dbContext;
        public CountryService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetCount()
        {
            return await _dbContext.Countries.CountAsync();
        }
        public async Task<bool> ChangedStatus(int id)
        {
            var country = await _dbContext.Countries.FirstAsync(x => x.Id == id);
            country.Status = !country.Status;
            await _dbContext.SaveChangesAsync();
            return country.Status;
        }

        public async Task<bool> CheckName(string name)
        {
            var rs = await _dbContext.Countries.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (rs == null) return false;
            return true;
        }

        public async Task<CountryViewModel?> Create(CountryViewModel model)
        {
            if (model == null) return null;
            var country = new Country
            {
                Name = model.Name,
                Description = model.Description,
                Slug = ConvertDatas.ConvertToSlug(model.Name),
                Status = true,
            };
            await _dbContext.Countries.AddAsync(country);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var country = await _dbContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null) return false;
            foreach (var movie in await _dbContext.Movies.Where(x => x.CountryId == country.Id).ToListAsync())
            {
                _dbContext.Movies.Remove(movie);
            }
            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CountryViewModel>> GetAll()
        {
            var query = from country in _dbContext.Countries
                        orderby country.Id descending
                        select new CountryViewModel
                        {
                            Id = country.Id,
                            Name = country.Name,
                            Description = country.Description,
                            Slug = country.Slug,
                            Status = country.Status,
                        };
            return await query.ToListAsync();
        }

        public async Task<CountryViewModel?> GetById(int id)
        {
            var query = from country in _dbContext.Countries
                        where country.Id == id
                        select new CountryViewModel
                        {
                            Id = country.Id,
                            Name = country.Name,
                            Description = country.Description,
                            Slug = country.Slug,
                            Status = country.Status,
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<CountryViewModel>> GetByStatusTrue()
        {
            var query = from country in _dbContext.Countries
                        orderby country.Name
                        where country.Status == true
                        select new CountryViewModel
                        {
                            Id = country.Id,
                            Name = country.Name,
                        };
            return await query.ToListAsync();
        }

        public async Task<CountryViewModel?> Update(int id, CountryViewModel model)
        {
            var country = await _dbContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null) return null;
            country.Name = model.Name;
            country.Description = model.Description;
            country.Slug = ConvertDatas.ConvertToSlug(model.Name);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
