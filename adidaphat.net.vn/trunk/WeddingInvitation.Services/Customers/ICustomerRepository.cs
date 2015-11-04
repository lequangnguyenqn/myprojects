using System.Collections.Generic;
using System.Linq;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Customers
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IQueryable<Customer> Search(string text);

        /// <summary>
        /// Get all customer in memmory cache
        /// </summary>
        /// <returns></returns>
        List<Customer> GetAllInMemmory();
    }
}
