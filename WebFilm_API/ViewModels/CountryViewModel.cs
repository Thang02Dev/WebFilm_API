using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebFilm_API.Models;

namespace WebFilm_API.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Tên quốc gia không được quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;
        [StringLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        [StringLength(100, ErrorMessage = "Slug không được quá 100 ký tự")]
        public string Slug { get; set; } = string.Empty;

    }
}
