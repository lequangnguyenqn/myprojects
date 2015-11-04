
using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Settings
{
    public class ShippingFee
    {
        [Key]
        public int ShippingFeeId { get; set; }
        public string ShippingFeeName { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public int MyOfficeId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
