using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCardModel>
{
    int IncrementCount(ShoppingCardModel cart, int count);
    int DecrementCount(ShoppingCardModel cart, int count);
}
