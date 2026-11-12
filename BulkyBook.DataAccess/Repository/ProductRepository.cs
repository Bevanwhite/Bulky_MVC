using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext db) 
    : Repository<Product>(db), IProductRepository
{
    private readonly ApplicationDbContext _db = db;

    public override void Update(Product obj)
    {
        _db.Products.Update(obj);
    }
}
