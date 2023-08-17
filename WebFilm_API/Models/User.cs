using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm_API.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        [Column(TypeName ="varchar")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [StringLength(200)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
