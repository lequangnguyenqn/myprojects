using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IImportDetailRepository : IRepository<ImportDetail>
    {
        IQueryable<ImportDetail> Search(string text);
    }
}
