using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm_API.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        public string? Trailer { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int Resolution { get; set; }
        public bool Subtitle { get; set; }
        public int Duration_Minutes { get; set; }
        [StringLength(250)]
        public string Image { get; set; } = string.Empty;
        [Column(TypeName = "varchar")]
        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public bool? Hot { get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        public string Name_Eng { get; set; } = string.Empty;
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Year_Release { get; set; } = string.Empty;
        public string? Tags { get; set; } = string.Empty;
        public bool? Top_View { get; set; }
        public int? Episode_Number { get; set; }
        public int? Position { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
