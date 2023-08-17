using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm_API.Models
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public int? Position { get; set; }
        public bool Status { get; set; }
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;
        public List<Movie>? Movies { get; set; }
    }
}
