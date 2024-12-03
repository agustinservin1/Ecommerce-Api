using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.Payments.Include(p => p.Order).ToListAsync();
        }
    }
}
