using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm_API.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;
        public List<Movie>? Movies { get; set; }
    }
}
