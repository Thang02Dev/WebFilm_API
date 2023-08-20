using System.ComponentModel.DataAnnotations;

namespace WebFilm_API.ViewModels
{
    public class LinkServerViewModel
    {
        public int Id { get; set; }
        [StringLength(250,ErrorMessage ="Tên server không quá 250 ký tự")]
        public string Name { get; set; } = string.Empty;
        [StringLength(250, ErrorMessage = "Mô tả không quá 250 ký tự")]
        public string? Description { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
