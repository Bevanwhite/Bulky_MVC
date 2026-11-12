using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    void Save();
}
