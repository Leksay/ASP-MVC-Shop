using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository;

public interface ICoverTypeRepository : IRepository<CoverType>
{
    public void Update(CoverType coverType);
}
