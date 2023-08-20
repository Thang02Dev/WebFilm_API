using System.ComponentModel.DataAnnotations;

namespace WebFilm_API.Models
{
    public class LinkServer
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [StringLength(250)]
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<Episode> Episodes { get; set; }
    }
}
