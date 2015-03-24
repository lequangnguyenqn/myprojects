using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IProductInStorageRepository : IRepository<ProductInStorage>
    {
        IQueryable<ProductInStorage> Search(string text);
    }
}
