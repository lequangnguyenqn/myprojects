using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public interface INewsItemRepository : IRepository<NewsItem>
    {
        IQueryable<NewsItem> Search(string text);
    }
}
