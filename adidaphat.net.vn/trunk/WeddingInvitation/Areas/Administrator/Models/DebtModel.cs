using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class DebtModel
    {
        public int DebtId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public int MyOfficeId { get; set; }
        public string MyOfficeName { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public decimal Total { get; set; }
        public string TotalDisplay { get { return String.Format("{0:0,0}", Total); } }
        public decimal Paid { get; set; }
        public string PaidDisplay { get { return String.Format("{0:0,0}", Paid); } }
        public decimal PaidLeft { get; set; }
        public string PaidLeftDisplay { get { return String.Format("{0:0,0}", PaidLeft); } }
    }
}