using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFilm_API.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [StringLength(100,ErrorMessage ="Tên danh mục không được quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;
        public int? Position { get; set; }
        public bool Status { get; set; }
        [StringLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string Description { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Slug không được quá 100 ký tự")]
        public string Slug { get; set; } = string.Empty;
    }
}
