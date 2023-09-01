using Azure.Core.GeoJson;
using Microsoft.EntityFrameworkCore;
using WebFilm_API.Commons;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.LinkServerServices
{
    public class LinkServerService : ILinkServerService
    {
        private readonly MyDbContext _dbContext;
        public LinkServerService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ChangedStatus(int id)
        {
            var link = await _dbContext.LinkServers.FirstAsync(x => x.Id == id);
            link.Status = !link.Status;
            await _dbContext.SaveChangesAsync();
            return link.Status;
        }

        public async Task<bool> CheckName(string name)
        {
            var rs = await _dbContext.LinkServers.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (rs == null) return false;
            return true;
        }

        public async Task<LinkServerViewModel?> Create(LinkServerViewModel model)
        {
            if (model == null) return null;
            var link = new LinkServer
            {
                Name = model.Name,
                Description = model.Description,
                Status = true,
            };
            await _dbContext.LinkServers.AddAsync(link);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var link = await _dbContext.LinkServers.FirstOrDefaultAsync(x => x.Id == id);
            if (link == null) return false;
            foreach (var item in await _dbContext.Episodes.Where(x => x.LinkServerId == link.Id).ToListAsync())
            {
                _dbContext.Episodes.Remove(item);
            }
            _dbContext.LinkServers.Remove(link);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<LinkServerViewModel>> GetAll()
        {
            var query = from link in _dbContext.LinkServers
                        orderby link.Id descending
                        select new LinkServerViewModel
                        {
                            Id = link.Id,
                            Name = link.Name,
                            Description = link.Description,                     
                            Status = link.Status,
                        };
            return await query.ToListAsync();
        }

        public async Task<LinkServerViewModel?> GetById(int id)
        {
            var query = from link in _dbContext.LinkServers
                        where link.Id == id
                        select new LinkServerViewModel
                        {
                            Id = link.Id,
                            Name = link.Name,
                            Description = link.Description,
                            Status = link.Status,
                        };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<LinkServerViewModel>> GetByStatus()
        {
            var query = from link in _dbContext.LinkServers
                        where link.Status == true
                        orderby link.Name
                        select new LinkServerViewModel
                        {
                            Id = link.Id,
                            Name = link.Name,
                        };
            return await query.ToListAsync();
        }

        public async Task<LinkServerViewModel?> Update(int id, LinkServerViewModel model)
        {
            var link = await _dbContext.LinkServers.FirstOrDefaultAsync(x => x.Id == id);
            if (link == null) return null;
            link.Name = model.Name;
            link.Description = model.Description;

            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
