using System.Linq.Expressions;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _dbSet = db.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public T? Get(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = _dbSet;
        return query.ToList();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
