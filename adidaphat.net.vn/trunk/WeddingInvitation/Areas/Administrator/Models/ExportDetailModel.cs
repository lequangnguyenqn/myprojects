using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ExportDetailModel
    {
        public int ExportDetailId { get; set; }
        public bool IsDeleted { get; set; }
        public int Amount { get; set; }
        public string AmountDisplay { get { return String.Format("{0:0,0}", Amount); } }
        public int ExportTrackId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StorageId { get; set; }
        public string CategoryName { get; set; }
    }
}