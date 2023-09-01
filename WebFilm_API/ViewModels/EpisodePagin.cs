namespace WebFilm_API.ViewModels
{
    public class EpisodePagin
    {
        public List<EpisodeViewModel> EpisodeViewModels { get; set; } = new List<EpisodeViewModel>();
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
