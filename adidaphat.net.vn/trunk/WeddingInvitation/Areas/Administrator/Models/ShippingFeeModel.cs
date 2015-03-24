using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ShippingFeeModel
    {
        public int ShippingFeeId { get; set; }
        public string ShippingFeeName { get; set; }
        public decimal Cost { get; set; }
        public string CostDisplay { get { return String.Format("{0:0,0}", Cost); } }
        public string Note { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> MyOffices { get; set; }
    }
}