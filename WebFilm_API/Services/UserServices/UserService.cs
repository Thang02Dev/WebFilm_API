using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebFilm_API.DB;
using WebFilm_API.Models;
using WebFilm_API.ViewModels;

namespace WebFilm_API.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(MyDbContext dbContext,IConfiguration configuration) 
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<bool> ChangedStatus(int id)
        {
            var user = await _dbContext.Users.FirstAsync(x => x.Id == id);
            user.Status = !user.Status;
            await _dbContext.SaveChangesAsync();
            return user.Status;
        }

        public async Task<bool> Create(UserViewModel model)
        {
            var isEmail = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Email==model.Email);
            if (isEmail!=null) return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                Password = passwordHash,
                Name = model.Name,
                Status = true,
                Created_Date = DateTime.Now,
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var rs = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (rs == null) return false;
            _dbContext.Users.Remove(rs);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public string GenerateToken(LoginViewModel model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,model.Email),
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<UserViewModel>?> GetAll()
        {
            var query = from u in _dbContext.Users
                        orderby u.Created_Date descending
                        select new UserViewModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Name = u.Name,
                            Password = u.Password,
                            Created_Date = u.Created_Date,
                            Updated_Date = u.Updated_Date,
                            Status = u.Status,
                        };
            return await query.ToListAsync();
        }

        public async Task<UserViewModel?> GetById(int id)
        {
            var query = from u in _dbContext.Users
                        where u.Id == id
                        select new UserViewModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Name = u.Name,
                            Created_Date = u.Created_Date,
                            Updated_Date = u.Updated_Date,
                            Status = u.Status,
                        };
            if (query == null) return null;
            return await query.FirstAsync();
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var result = await _dbContext.Users.SingleOrDefaultAsync(x=>x.Email==model.Email
                                                                    && x.Status == true);
            if (result == null) return false;
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(model.Password, result.Password)) return false;
                
                return true;
            }
            
        }

        public async Task<UserViewModel?> Update(int id, UserViewModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return null;
            user.Name = model.Name;
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.Updated_Date = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
