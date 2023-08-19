using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFilm_API.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessage = "Tên người dùng không được quá 250 ký tự")]
        public string Name { get; set; } = string.Empty;
        [StringLength(200,ErrorMessage ="Email không được quá 200 ký tự")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        [StringLength(200, ErrorMessage = "Mật khẩu không được quá 200 ký tự")]
        public string Password { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
    }
}
