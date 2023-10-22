namespace WebFilm_API.Models
{
    public class View
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string? ViewerIP { get; set; }
        public DateTime? DateViewed { get; set; }
        public Movie Movie { get; set; }
    }
}
