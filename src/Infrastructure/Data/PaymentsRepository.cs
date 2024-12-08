using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PaymentsRepository : BaseRepository<Payments>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentsRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Payments>> GetAllPaymentsRepository()
        {
            return await _context.Payments.ToListAsync();
        }
        public async Task<Payments> GetPaymentById(int paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }
    }
}
