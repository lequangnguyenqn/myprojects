using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Tên tối đa là 128 kí tự.")]
        public string CustomerName { get; set; }
        //Chanh xe
        [Required(ErrorMessage = "Chành xe là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Chành xe tối đa là 128 kí tự.")]
        public string CustomerShortName { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc nhập.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email không hợp lệ")]
        [StringLength(128, ErrorMessage = "Email hàng tối đa là 128 kí tự.")]
        public string CustomerCode { get; set; }
        //So nha - Ten duong
        [Required(ErrorMessage = "Số nhà/tên đường là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Số nhà/tên đường tối đa là 128 kí tự.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Điện thoại là bắt buộc nhập.")]
        [StringLength(24, ErrorMessage = "Số điện thoại tối đa là 24 kí tự.")]
        public string PhoneNumber { get; set; }
        //Xa Phuong
        [Required(ErrorMessage = "Xã/Phường là bắt buộc nhập.")]
        [StringLength(24, ErrorMessage = "Xã/Phường tối đa là 24 kí tự.")]
        public string Fax { get; set; }
        //Quan Huyen
        [Required(ErrorMessage = "Quận/huyện là bắt buộc nhập.")]
        [StringLength(24, ErrorMessage = "Quận/huyện tối đa là 24 kí tự.")]
        public string CellPhoneNumber { get; set; }
        public string Note { get; set; }
        public int DiscountPercent { get; set; }
        public bool UseSpecialRateTable { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public bool HideWithNormalUser { get; set; }
    }
}