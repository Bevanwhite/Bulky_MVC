using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product obj);
}
