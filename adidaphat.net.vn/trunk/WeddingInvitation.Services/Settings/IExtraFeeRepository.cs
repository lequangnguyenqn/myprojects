using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IExtraFeeRepository : IRepository<ExtraFee>
    {
        IQueryable<ExtraFee> Search(string text);
    }
}
