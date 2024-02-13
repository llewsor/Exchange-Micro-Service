using System.Linq.Expressions;

namespace ExchangeService.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T?> GetByCondition(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}