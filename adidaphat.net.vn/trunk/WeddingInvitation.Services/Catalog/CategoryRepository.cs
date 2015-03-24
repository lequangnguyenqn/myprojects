using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Catalog
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<Category> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.CategoryName.Contains(text) || p.CategoryCode.Contains(text)));
        }
    }
}
