using System.Data.Entity;

namespace WeddingInvitation.Services.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private bool _rollback;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public DbContext DbContext()
        {
            return _dbContext;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            _rollback = true;
        }

        public void Dispose()
        {
            if (!_rollback)
            {
                Commit();
            }
        }     
    }
}