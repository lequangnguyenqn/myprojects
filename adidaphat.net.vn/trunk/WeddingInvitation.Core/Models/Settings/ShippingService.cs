using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Settings
{
    public class ShippingService
    {
        [Key]
        public int ShippingServiceId { get; set; }
        [StringLength(128)]
        public string ShippingServiceName { get; set; }
        [StringLength(128)]
        public string Address { get; set; }
        [StringLength(24)]
        public string PhoneNumber { get; set; }
        [StringLength(24)]
        public string CellPhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int MyOfficeId { get; set; }
        public DateTime StartAt { get; set; }
        public string CoachStation { get; set; }
    }
}
