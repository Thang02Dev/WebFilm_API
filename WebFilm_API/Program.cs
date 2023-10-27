using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebFilm_API.DB;
using WebFilm_API.Hubs;
using WebFilm_API.Services.CategoryServices;
using WebFilm_API.Services.CountryServices;
using WebFilm_API.Services.EpisodeServices;
using WebFilm_API.Services.GenreServices;
using WebFilm_API.Services.LinkServerServices;
using WebFilm_API.Services.MovieServices;
using WebFilm_API.Services.UserServices;
using WebFilm_API.Services.ViewServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICountryService,CountryService>();
builder.Services.AddScoped<IGenreService,GenreService>();
builder.Services.AddScoped<IMovieService,MovieService>();
builder.Services.AddScoped<ILinkServerService,LinkServerService>();
builder.Services.AddScoped<IEpisodeService,EpisodeService>();
builder.Services.AddScoped<IViewService,ViewService>();
builder.Services.AddScoped<IUserService,UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:SecretKey").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });



builder.Services.AddCors(c => c.AddPolicy("AlowOrigin", options => options.WithOrigins("http://127.0.0.1:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<UserHub>("/userhub");
app.MapControllers();

app.Run();
