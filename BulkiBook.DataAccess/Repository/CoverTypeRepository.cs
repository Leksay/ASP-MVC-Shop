using BulkiBook.DataAcces;
using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository;

public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
{
    private readonly ApplicationDBContext _db;

    public CoverTypeRepository(ApplicationDBContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CoverType coverType)
    {
        _db.CoverTypes.Update(coverType);
    }
}
