using System.Linq;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Settings
{
    public class RateMappingRepository : RepositoryBase<RateMapping>, IRateMappingRepository
    {
        public RateMappingRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<RateMapping> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false).OrderByDescending(p => p.RateMappingId)
                                : GetAll()
                                .Where(p => p.IsDeleted == false).OrderByDescending(p => p.RateMappingId);
        }
    }
}
