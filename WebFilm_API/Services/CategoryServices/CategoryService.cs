﻿using Microsoft.EntityFrameworkCore;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly MyDbContext _dbContext;
        public CategoryService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ChangedStatus(int id)
        {
            var cate = await _dbContext.Categories.FirstAsync(x => x.Id == id);
            cate.Status = !cate.Status;
            await _dbContext.SaveChangesAsync();
            return cate.Status;
        }

        public async Task<bool> CheckName(string name)
        {
            var rs = await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Name.ToLower()==name.ToLower());
            if (rs == null) return false;
            return true;
        }

        public async Task<CategoryViewModel?> Create(CategoryViewModel model)
        {
            if (model == null) return null;
            var cate = new Category
            {
                Name = model.Name,
                Description = model.Description,
                Position = model.Position,
                Slug = model.Slug,
                Status = model.Status,
            };
            await _dbContext.Categories.AddAsync(cate);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var cate = await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Id==id);
            if (cate == null) return false;
            foreach(var movie in await _dbContext.Movies.Where(x=>x.CategoryId==cate.Id).ToListAsync())
            {
                _dbContext.Movies.Remove(movie);
            }
            _dbContext.Categories.Remove(cate);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            var query = from cate in _dbContext.Categories
                        orderby cate.Id descending
                        select new CategoryViewModel
                        {
                            Id = cate.Id,
                            Name = cate.Name,
                            Description = cate.Description,
                            Position = cate.Position,
                            Slug = cate.Slug,
                            Status = cate.Status,
                        };
            return await query.ToListAsync();
        }

        public async Task<CategoryViewModel?> GetById(int id)
        {
            var query = from cate in _dbContext.Categories
                        where cate.Id == id
                        select new CategoryViewModel
                        {
                            Id = cate.Id,
                            Name = cate.Name,
                            Description = cate.Description,
                            Position = cate.Position,
                            Slug = cate.Slug,
                            Status = cate.Status,
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<CategoryViewModel?> Update(int id, CategoryViewModel model)
        {
            var cate = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (cate == null) return null;
            cate.Name = model.Name;
            cate.Description = model.Description;
            cate.Status = model.Status;
            cate.Position = model.Position;
            cate.Slug = model.Slug;
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
