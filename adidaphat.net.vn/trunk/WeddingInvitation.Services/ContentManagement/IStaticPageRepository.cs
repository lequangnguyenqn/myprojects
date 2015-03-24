using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public interface IStaticPageRepository : IRepository<StaticPage>
    {
        IQueryable<StaticPage> Search(string text);
    }
}
