using System;
using System.Data.Entity;

namespace WeddingInvitation.Services.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbContext DbContext();
        int Commit();
        void Rollback();
    }
}