using BulkiBook.DataAcces;
using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository 
{
    private readonly ApplicationDBContext _db;

    public ProductRepository(ApplicationDBContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Product product)
    {
        var productFromDB = _db.Products.FirstOrDefault(p => p.Id == product.Id);

        if(productFromDB != null)
        {
            productFromDB.Title = product.Title;
            productFromDB.Description = product.Description;
            productFromDB.Category = product.Category;
            productFromDB.ISBN = product.ISBN;
            productFromDB.ListPrice = product.ListPrice;
            productFromDB.Price = product.Price;
            productFromDB.Price50 = product.Price50;
            productFromDB.Price100 = product.Price100;
            productFromDB.Author = product.Author;
            productFromDB.CategoryId = product.CategoryId;
            productFromDB.CoverTypeId = product.CoverTypeId;

            if(product.ImageUrl != null)
            {
                productFromDB.ImageUrl = product.ImageUrl;
            }
        }
    }
}
