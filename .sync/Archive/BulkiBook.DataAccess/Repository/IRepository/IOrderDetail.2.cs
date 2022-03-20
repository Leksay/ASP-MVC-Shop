using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetail : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
