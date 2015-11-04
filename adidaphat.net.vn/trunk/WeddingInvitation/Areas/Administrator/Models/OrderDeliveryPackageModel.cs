using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class OrderDeliveryPackageModel
    {
        public int OrderDeliveryPackageId { get; set; }
        public int MyOfficeId { get; set; }
        public string MyOfficeName { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string Orders { get; set; }
        public int ShippingFeeId { get; set; }
        public decimal ShippingFee { get; set; }
        public string ShippingFeeDisplay { get { return String.Format("{0:0,0}", ShippingFee); } }
        public List<ShippingFee> ShippingFees { get; set; }
        public int DeliveryUserId { get; set; }
        public string DeliveryUserName { get; set; }
        public List<User> Users { get; set; }
        public string Note { get; set; }
    }
}