using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class MyOfficeModel
    {
        public int MyOfficeId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Tên tối đa là 128 kí tự.")]
        public string OfficeName { get; set; }
        [StringLength(128, ErrorMessage = "Địa chỉ tối đa là 128 kí tự.")]
        public string Address { get; set; }
        [StringLength(24, ErrorMessage = "Số điện thoại tối đa là 128 kí tự.")]
        public string PhoneNumber { get; set; }
        [StringLength(24, ErrorMessage = "Fax tối đa là 128 kí tự.")]
        public string Fax { get; set; }
        public bool IsRetailCustomer { get; set; }
        public int StorageId { get; set; }
        public List<Storage> Storages { get; set; }
    }
}