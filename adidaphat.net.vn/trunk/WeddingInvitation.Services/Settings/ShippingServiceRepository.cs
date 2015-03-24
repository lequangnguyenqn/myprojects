using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public class ShippingServiceRepository : RepositoryBase<ShippingService>, IShippingServiceRepository
    {
        public ShippingServiceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<ShippingService> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.ShippingServiceName.Contains(text) || p.Address.Contains(text)));
        }
    }
}
