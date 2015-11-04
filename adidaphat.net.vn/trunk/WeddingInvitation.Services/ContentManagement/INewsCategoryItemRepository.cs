using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public interface INewsCategoryItemRepository : IRepository<NewsCategoryItem>
    {
        IQueryable<NewsCategoryItem> Search(string text);
        NewsCategoryItem GetRootCategory(NewsCategoryItem category);
        IQueryable<NewsCategoryItem> GetChildren(int catId);
    }
}
