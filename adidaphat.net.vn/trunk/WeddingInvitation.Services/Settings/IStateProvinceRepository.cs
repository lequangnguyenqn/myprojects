using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IStateProvinceRepository : IRepository<StateProvince>
    {
        IQueryable<StateProvince> Search(string text);
    }
}
