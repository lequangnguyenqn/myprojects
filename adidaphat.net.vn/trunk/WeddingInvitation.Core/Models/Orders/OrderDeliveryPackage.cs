using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Core.Models.Orders
{
    public class OrderDeliveryPackage
    {
        [Key]
        public int OrderDeliveryPackageId { get; set; }
        [ForeignKey("MyOffice")]
        public int MyOfficeId { get; set; }
        public virtual MyOffice MyOffice { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public ICollection<Order> Orders { get; set; }
        public int ShippingFeeId { get; set; }
        public decimal ShippingFee { get; set; }
        public int DeliveryUserId { get; set; }
        public bool IsDeleted { get; set; }
        public string Note { get; set; }
        public int CustomerId { get; set; }
    }
}
