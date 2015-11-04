using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IStorageRepository : IRepository<Storage>
    {
        IQueryable<Storage> Search(string text);
    }
}
