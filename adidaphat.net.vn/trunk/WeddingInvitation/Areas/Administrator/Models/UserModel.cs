using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Data;
using WeddingInvitation.Data.Mapping.Users;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class UserModel
    {
        public UserModel()
        {
            BelongOffices = new List<int>();
            BelongStorages = new List<int>();
        }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Họ là bắt buộc nhập."), StringLength(50, ErrorMessage = "Họ tối đa là 50 kí tự.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập."), StringLength(50, ErrorMessage = "Tên tối đa là 50 kí tự.")]
        public string LastName { get; set; }

        public string FullName {
            get { return string.Format("{0} {1}", FirstName, LastName); } 
        }

        [Required(ErrorMessage = "Email là bắt buộc nhập.")]
        //[Remote("IsEmailAvailable", "User", AdditionalFields = "UserId", ErrorMessage = "Email đã tồn tại.")]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập.")]
        [StringLength(50, ErrorMessage = "{0} phải có độ dài ít nhất là {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }

        public bool IsLockedOut { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreateDate { get; set; }
        public string Address { get; set; }
        public List<Role> AvailableUserRoles { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<MyOffice> AvailableOffices { get; set; }
        public List<int> BelongOffices { get; set; }
        public int MyOfficeId { get; set; }
        [DataType(DataType.Time)]
        public DateTime? AllowLoginFrom { get; set; }
        [DataType(DataType.Time)]
        public DateTime? AllowLoginTo { get; set; }
        public List<Storage> AvailableStorages { get; set; }
        public List<int> BelongStorages { get; set; }
        public decimal Salary { get; set; }
        public bool DeliveryInDay { get; set; }
    }
}