using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Customers
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private readonly ICacheManager _cacheManager;
        private const string ALLCUSTOMERS = "WeddingInvitation.Services.Customers.AllCustomers";

        public CustomerRepository(IUnitOfWork unitOfWork, ICacheManager cacheManager)
            : base(unitOfWork)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<Customer> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false).OrderByDescending(p => p.CustomerId)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.CustomerName.Contains(text) || p.Address.Contains(text) || p.PhoneNumber.Contains(text))).OrderByDescending(p => p.CustomerId);
        }

        public List<Customer> GetAllInMemmory()
        {
            string key = ALLCUSTOMERS;
            return _cacheManager.Get(key, () =>
            {
                var query = GetAll().ToList();
                return query;
            });
        }

        //public Customer Update(Customer entity)
        //{
        //    Contract.Requires(entity != null);

        //    DbEntityEntry<Customer> entityEntry = this._dbContext.Entry<Customer>(entity);
        //    if (entityEntry.State == EntityState.Detached)
        //    {
        //        this._dbSet.Attach(entity);
        //    }
        //    entityEntry.State = EntityState.Modified;
        //    _cacheManager.RemoveByPattern(ALLCUSTOMERS);
        //    return entity;
        //}
    }
}
