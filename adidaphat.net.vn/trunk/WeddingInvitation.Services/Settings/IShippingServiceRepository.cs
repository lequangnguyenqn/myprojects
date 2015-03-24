using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IShippingServiceRepository : IRepository<ShippingService>
    {
        IQueryable<ShippingService> Search(string text);
    }
}
