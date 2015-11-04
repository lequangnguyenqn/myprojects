using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;

namespace WeddingInvitation.Services.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected DbContext _dbContext;
        protected IDbSet<T> _dbSet;

        protected RepositoryBase(IUnitOfWork unitOfWork)
        {
            _dbSet = unitOfWork.Set<T>();
            _dbContext = unitOfWork.DbContext();
        }
        protected RepositoryBase() { }

        #region IRepository<T> Members

        /// <summary>
        /// Get all with paging support
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Get first element
        /// </summary>
        /// <returns></returns>
        public virtual T First()
        {
            return _dbSet.FirstOrDefault();
        }

        /// <summary>
        /// Get element by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(params object[] id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Delete element
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete element by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(params object[] id)
        {
            var obj = GetById(id);
            if (obj != null)
            {
                Delete(obj);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Add Or Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        public virtual void AddOrUpdate(T entity, params object[] id)
        {
            if (id.Count() == 0 || GetById(id) == null)
            {
                _dbSet.Add(entity);
            }
        }

        /// <summary>
        /// Insert a element
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            Contract.Requires(entity != null);

            DbEntityEntry<T> entityEntry = this._dbContext.Entry<T>(entity);
            if (entityEntry.State != EntityState.Detached)
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this._dbSet.Add(entity);
            }
            return entity;
        }


        /// <summary>
        /// Update a element
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            Contract.Requires(entity != null);

            DbEntityEntry<T> entityEntry = this._dbContext.Entry<T>(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }
            entityEntry.State = EntityState.Modified;

            return entity;
        }
        #endregion

    }
}
