using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.Core.Models.Orders;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class PaymentPeriodModel
    {
        public int PaymentPeriodId { get; set; }
        public decimal Paid { get; set; }
        public string PaidDisplay { get { return String.Format("{0:0,0}", Paid); } }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public int MyOfficeId { get; set; }
        public int OrderId { get; set; }
        public string ApproveFromManagerName { get; set; }
        public string MyOfficeName { get; set; }
        public string CustomerName { get; set; }
        public int PaymentType { get; set; }
        public string InShippingPlaceDisplay
        {
            get
            {
                if (PaymentType == (int)PaymentTypes.InShippingPlace)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public string TranferDisplay
        {
            get
            {
                if (PaymentType == (int)PaymentTypes.Tranfer)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
    }
}