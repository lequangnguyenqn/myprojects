using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;
using System.Data.Entity;

namespace WeddingInvitation.Services.Storages
{
    public class ProductInStorageRepository : RepositoryBase<ProductInStorage>, IProductInStorageRepository
    {
        public ProductInStorageRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<ProductInStorage> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll().Include(p => p.Product).Include(p => p.Storage)
                                .Where(p => (p.Product.ProductName.Contains(text) || p.Storage.StorageName.Contains(text)) && p.IsDeleted == false);
        }
    }
}
