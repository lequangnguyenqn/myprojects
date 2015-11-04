using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Customers
{
    public class CustomerLogoRepository : RepositoryBase<CustomerLogo>, ICustomerLogoRepository
    {
        public CustomerLogoRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<CustomerLogo> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().OrderByDescending(p => p.CustomerLogoId)
                                : GetAll()
                                .Where(p => p.CustomerName.Contains(text)).OrderByDescending(p => p.CustomerLogoId);
        }
    }
}
