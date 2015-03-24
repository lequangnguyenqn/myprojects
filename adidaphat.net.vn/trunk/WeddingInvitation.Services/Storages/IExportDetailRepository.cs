using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IExportDetailRepository : IRepository<ExportDetail>
    {
        IQueryable<ExportDetail> Search(string text);
    }
}
