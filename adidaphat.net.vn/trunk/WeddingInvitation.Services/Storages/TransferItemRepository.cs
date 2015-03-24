using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public class TransferItemRepository : RepositoryBase<TransferItem>, ITransferItemRepository
    {
        public TransferItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<TransferItem> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false)
                                : GetAll()
                                .Where(p => p.IsDeleted == false);
        }
    }
}
