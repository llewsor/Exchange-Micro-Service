using ExchangeService.Entity;
using ExchangeService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExchangeService.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public ExchangeDbContext Context { get; set; }

        public BaseRepository(ExchangeDbContext context)
        {
            Context = context;
        }

        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public Task<T?> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().AnyAsync(expression);
        }
    }
}
