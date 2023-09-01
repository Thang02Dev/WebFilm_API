using System.ComponentModel.DataAnnotations;
using WebFilm_API.Models;

namespace WebFilm_API.ViewModels
{
    public class EpisodeViewModel
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public string ServerName { get; set; } = string.Empty;
        [StringLength(250,ErrorMessage ="Chuỗi link phim không quá 250 ký tự")]
        public string Link { get; set; } = string.Empty;
        public int Episode_Number { get; set; } //VD: Tập 1, Tập 2,...
        public int LinkServerId { get; set; }
    }
}
