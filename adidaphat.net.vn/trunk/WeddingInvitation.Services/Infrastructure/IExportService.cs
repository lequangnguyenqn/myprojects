using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Orders;

namespace WeddingInvitation.Services.Infrastructure
{
    public interface IExportService
    {
        MemoryStream DownloadDeliveryPackage(ReportFilter reportFilter);
        MemoryStream DownloadOrderReporting(List<Order> orders, string from, string to, int[] offices, int[] products);
        MemoryStream DownloadPaymentPeriod(IQueryable<Order> model, ReportFilter reportFilter);
    }
}
