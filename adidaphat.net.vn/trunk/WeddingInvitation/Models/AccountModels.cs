using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WeddingInvitation.Infrastructure.Mvc.Validators;

namespace WeddingInvitation.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public bool NeedChangePassWord { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [EmailValidation(ErrorMessage = "The email is invalid")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountDetailsModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "UserName")]
        public string ContactName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailValidation(ErrorMessage = "The email is invalid")]
        [Display(Name = "Email Address")]
        [Remote("IsEmailAvailable", "Account", AdditionalFields = "UserId", ErrorMessage = "the email already exists")]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        [StringLength(128)]
        [Display(Name = "address 1")]
        public string Address1 { get; set; }
        [StringLength(128)]
        [Display(Name = "address 2")]
        public string Address2 { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Town / City ")]
        public string TownCity { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Post Code / Zip")]
        public string PostCodeZip { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Country / Region ")]
        public string CountryRegion { get; set; }
        public int PackageId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountForListModel
    {
        public int AccountId { get; set; }
        public string CompanyName { get; set; }
        public string PackageName { get; set; }
        public int Sites { get; set; }
        public int URLS { get; set; }
        public int PageViews { get; set; }
        public int DataAllowance { get; set; }
        public bool IsDisable { get; set; }
        public ICollection<UserForListModel> Users { get; set; }
    }

    public class UserForListModel
    {
        public int UserId { get; set; }
        public string ContactName { get; set; }
        public string RoleName { get; set; }
        public bool IsLockedOut { get; set; }
    }
}