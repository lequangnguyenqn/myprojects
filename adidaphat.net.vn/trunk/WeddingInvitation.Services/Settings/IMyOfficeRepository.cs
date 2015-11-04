using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public interface IMyOfficeRepository : IRepository<MyOffice>
    {
        IQueryable<MyOffice> Search(string text);
    }
}
