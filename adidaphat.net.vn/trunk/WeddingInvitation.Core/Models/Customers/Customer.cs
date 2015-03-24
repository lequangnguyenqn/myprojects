using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Customers
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string CustomerName { get; set; }
        //Chanh xe
        [StringLength(128)]
        public string CustomerShortName { get; set; }
        [StringLength(128)]
        public string CustomerCode { get; set; }
        //So nha - Ten duong
        [StringLength(128)]
        public string Address { get; set; }
        [StringLength(24)]
        public string PhoneNumber { get; set; }
        //Xa Phuong
        [StringLength(24)]
        public string Fax { get; set; }
        //Quan Huyen
        [StringLength(24)]
        public string CellPhoneNumber { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
        public bool DoNotDelete { get; set; }
        public int MyOfficeId { get; set; }
        public int DiscountPercent { get; set; }
        public bool UseSpecialRateTable { get; set; }
        public bool HideWithNormalUser { get; set; }
        public bool Trusted { get; set; }
    }
}
