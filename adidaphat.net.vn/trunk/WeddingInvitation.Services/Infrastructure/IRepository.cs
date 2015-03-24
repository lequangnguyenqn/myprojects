using System.Linq;

namespace WeddingInvitation.Services.Infrastructure
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();
        T First();
        T GetById(params object[] id);
        void Delete(T entity);
        bool Delete(params object[] id);
        void AddOrUpdate(T entity, params object[] id);
        T Insert(T entity);
        T Update(T entity);
    }
}
