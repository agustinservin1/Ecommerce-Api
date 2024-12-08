using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payments>
    {
        Task<IEnumerable<Payments>> GetAllPaymentsRepository();
        Task<Payments> GetPaymentById(int paymentId);
    }
}
