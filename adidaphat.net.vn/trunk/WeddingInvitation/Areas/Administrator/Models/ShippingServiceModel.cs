using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ShippingServiceModel
    {
        public int ShippingServiceId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string ShippingServiceName { get; set; }
        [StringLength(128)]
        public string Address { get; set; }
        [StringLength(24)]
        public string PhoneNumber { get; set; }
        [StringLength(24)]
        public string CellPhoneNumber { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        [DataType(DataType.Time)]
        public DateTime StartAt { get; set; }
        public string CoachStation { get; set; }
    }
}