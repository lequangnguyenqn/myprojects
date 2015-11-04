using System.Linq;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Storages
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        IQueryable<Expense> Search(string text);
    }
}
