namespace WebFilm_API.ViewModels
{
    public class MoviePagin
    {
        public List<MovieViewModel> MovieViewModels { get; set; } = new List<MovieViewModel>();
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
