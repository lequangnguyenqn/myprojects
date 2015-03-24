using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IShippingFeeRepository : IRepository<ShippingFee>
    {
        IQueryable<ShippingFee> Search(string text);
    }
}
