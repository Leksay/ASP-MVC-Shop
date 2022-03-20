using BulkiBook.DataAcces;
using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCardModel>, IShoppingCartRepository
{
    private ApplicationDBContext _db;

    public ShoppingCartRepository(ApplicationDBContext db) : base(db)
    {
        _db = db;
    }

    public int DecrementCount(ShoppingCardModel cart, int count)
    {
        cart.Count -= Math.Clamp(count, 0, cart.Count);
        return cart.Count;
    }

    public int IncrementCount(ShoppingCardModel cart, int count)
    {
        cart.Count += count;
        return cart.Count;
    }
}
