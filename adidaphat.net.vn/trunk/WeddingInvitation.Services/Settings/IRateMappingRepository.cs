using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IRateMappingRepository : IRepository<RateMapping>
    {
        IQueryable<RateMapping> Search(string text);
    }
}
