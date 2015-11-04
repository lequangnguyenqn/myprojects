using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public class StaticPageRepository : RepositoryBase<StaticPage>, IStaticPageRepository
    {
        public StaticPageRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<StaticPage> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.Description.Contains(text)));
        }
    }
}
