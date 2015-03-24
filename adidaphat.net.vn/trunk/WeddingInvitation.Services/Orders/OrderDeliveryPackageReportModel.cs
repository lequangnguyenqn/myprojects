using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Services.Orders
{
    public class OrderDeliveryPackageReportModel
    {
        public int OrderDeliveryPackageId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public List<ProductReportModel> ProductReportModels { get; set; }
        public decimal Total { get; set; }
        public int PaymentType { get; set; }
        public string ShippingServiceName { get; set; }
        public string ShippingServicePhone { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? StartAt { get; set; }
    }
    public class ProductReportModel
    {
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
