using Microsoft.EntityFrameworkCore;
using WebFilm_API.DB;
using WebFilm_API.Services.CategoryServices;
using WebFilm_API.Services.CountryServices;
using WebFilm_API.Services.GenreServices;
using WebFilm_API.Services.MovieService;
using WebFilm_API.Services.MovieServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();

builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICountryService,CountryService>();
builder.Services.AddScoped<IGenreService,GenreService>();
builder.Services.AddScoped<IMovieService,MovieService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
