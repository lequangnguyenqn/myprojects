using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Settings
{
    public class MyOffice
    {
        [Key]
        public int MyOfficeId { get; set; }
        [StringLength(128)]
        public string OfficeName { get; set; }
        [StringLength(128)]
        public string Address { get; set; }
        [StringLength(24)]
        public string PhoneNumber { get; set; }
        [StringLength(24)]
        public string Fax { get; set; }
        [StringLength(24)]
        public string CellPhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRetailCustomer { get; set; }
        public ICollection<User> Users { get; set; }
        public int StorageId { get; set; }
    }
}
