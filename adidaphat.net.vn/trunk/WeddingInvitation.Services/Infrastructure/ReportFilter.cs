using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Services.Infrastructure
{
    public class ReportFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] ListOffices { get; set; }
        public int[] AllowListOffices { get; set; }
    }
}
