using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public class NewsItemRepository : RepositoryBase<NewsItem>, INewsItemRepository
    {
        public NewsItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<NewsItem> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false).OrderByDescending(p => p.NewsItemId)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.Title.Contains(text) || p.Short.Contains(text))).OrderByDescending(p => p.NewsItemId);
        }
    }
}
