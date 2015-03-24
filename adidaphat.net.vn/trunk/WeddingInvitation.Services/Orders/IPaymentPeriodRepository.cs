using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Orders
{
    public interface IPaymentPeriodRepository : IRepository<PaymentPeriod>
    {
        IQueryable<PaymentPeriod> Search(string text);
    }
}
