using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Catalog
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> Search(string text);
    }
}
