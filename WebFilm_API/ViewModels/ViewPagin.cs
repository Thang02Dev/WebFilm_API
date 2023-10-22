namespace WebFilm_API.ViewModels
{
    public class ViewPagin
    {
        public List<ViewViewModel> ViewViewModels { get; set; } = new List<ViewViewModel>();
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
