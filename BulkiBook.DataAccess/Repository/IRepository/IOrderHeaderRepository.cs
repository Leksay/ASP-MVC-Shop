using BulkiBook.Models;

namespace BulkiBook.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader obj);
    void UpdateStatus(int id, string status, string? paymentStatus = null);
    public void UpdateStripePaymentId(int id, string sessionId, string paymentIntendId);
}