using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Orders
{
    public class OrderDeliveryPackageRepository : RepositoryBase<OrderDeliveryPackage>, IOrderDeliveryPackageRepository
    {
        public OrderDeliveryPackageRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<OrderDeliveryPackage> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll()
                                .Where(p => p.IsDeleted == false);
        }
    }
}
