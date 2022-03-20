using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeader : IRepository<OrderHeader>
    {
        void Update(Category obj);
    }
}
