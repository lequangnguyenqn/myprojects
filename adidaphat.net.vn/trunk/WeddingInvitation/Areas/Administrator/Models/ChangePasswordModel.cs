using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập.")]
        [StringLength(100, ErrorMessage = "{0} phải có độ dài ít nhất là {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
}