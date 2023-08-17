using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm_API.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int? MovieId {get; set; }
        [Column(TypeName = "varchar")]
        [StringLength(250)]
        public string Link { get; set; } = string.Empty;
        public int Episode_Number { get; set; }
        public Movie? Movie { get; set; }

    }
}
