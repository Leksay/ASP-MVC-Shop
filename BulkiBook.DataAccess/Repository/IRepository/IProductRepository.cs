using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
    public void Update(Product coverType);
}
