using Domain.Entities;
using MercadoPago.Resource.Preference;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Preference> CreatePaymentAsync(int idOrder);
        Task<IEnumerable<Payments>> GetAllPayments();
        Task<Payments> GetPaymentById(int id);
        Task<IEnumerable<Payments>>GetApprovedPayments();
    }
}
