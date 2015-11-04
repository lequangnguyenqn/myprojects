using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Customers
{
    public interface ICustomerLogoRepository : IRepository<CustomerLogo>
    {
        IQueryable<CustomerLogo> Search(string text);
    }
}
