
namespace WebFilm_API.ViewModels
{
    public class ViewViewModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Name_Eng { get; set; }
        public int Count { get; set; }
        public string Slug { get; set; }
        public string? ViewerIP { get; set; }
        public DateTime? DateViewed { get; set; }
    }
}
