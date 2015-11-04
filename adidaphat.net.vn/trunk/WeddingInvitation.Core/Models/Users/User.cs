using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Core.Models.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        [StringLength(50)]
        [Display(Name = "Contact Number")]
        public string ContactNumber  { get; set; }
        [StringLength(128)]
        [Display(Name = "address 1")]
        public string Address1 { get; set; }
        [StringLength(128)]
        [Display(Name = "address 2")]
        public string Address2 { get; set; }
        [StringLength(50)]
        [Display(Name = "Town / City ")]
        public string TownCity  { get; set; }
        [StringLength(50)]
        [Display(Name = "Post Code / Zip")]
        public string PostCodeZip { get; set; }
        [StringLength(10)]
        [Display(Name = "Country / Region ")]
        public string CountryRegion  { get; set; }
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public decimal Salary { get; set; }
        
        [StringLength(50)]
        public string Username { get; set; }
        
        [StringLength(50)]
        public string PasswordSalt { get; set; }
        public bool IsApproved { get; set; }
        
        public bool IsLockedOut { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOnline { get; set; }
        public bool NeedChangePassWord { get; set; }
        public bool NeedRemindChangePassword { get; set; }
        
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogInDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastLockedOutDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public DateTime? AllowLoginFrom { get; set; }
        public DateTime? AllowLoginTo { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public string ProfileImage { get; set; }
        public string UserImage { get; set; }
        [StringLength(50)]
        public string TimeZone { get; set; }
        [StringLength(25)]
        public string ColourScheme { get; set; }
        public virtual ICollection<MyOffice> MyOffices { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }
        [NotMapped]
        public string FullName { get { return String.Format("{0} {1}", FirstName, LastName); } }
    }
}
