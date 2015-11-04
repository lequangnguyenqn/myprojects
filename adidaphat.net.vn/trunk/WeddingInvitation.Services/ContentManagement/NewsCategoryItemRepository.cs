using System.Collections.Generic;
using System.Linq;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.ContentManagement
{
    public class NewsCategoryItemRepository : RepositoryBase<NewsCategoryItem>, INewsCategoryItemRepository
    {
        public NewsCategoryItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public NewsCategoryItem GetRootCategory(NewsCategoryItem category)
        {
            if (category.Parent == null)
                return category;
            return GetRootCategory(category.Parent);
        }

        public IQueryable<NewsCategoryItem> Search(string text)
        {
            var lstCategories = GetAll().Where(p => p.IsDeleted == false).OrderBy(p => p.DisplayOrder).ToList();
            foreach (var category in lstCategories)
            {
                category.Children = lstCategories.Where(p => p.Parent != null && p.Parent.NewsCategoryItemId == category.NewsCategoryItemId).ToList();
            }
            List<NewsCategoryItem> lstTmp = new List<NewsCategoryItem>();
            var root = new NewsCategoryItem()
            {
                NewsCategoryItemId = -1,
                CategoryName = "Categories",
                Active = true,
                Children = lstCategories.Where(p => p.Level == 0).ToList(),
            };
            lstTmp.Add(root);
            return lstTmp.AsQueryable();
        }

        public IQueryable<NewsCategoryItem> GetChildren(int catId)
        {
            return GetAll().Where(p => p.ParentId == catId);
        }
    }
}
