using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetail : IRepository<Category>
    {
        void Update(Category obj);
    }
}
