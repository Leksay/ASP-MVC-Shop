using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeader : IRepository<Category>
    {
        void Update(Category obj);
    }
}
