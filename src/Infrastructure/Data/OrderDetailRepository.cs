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
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OrderDetail> GetByIdOrderDetails(int id)
        {
            var orderDetail = await _context.OrderDetails
                                            .Include(oc => oc.Order)
                                            .Include(oc => oc.Product)
                                            .FirstOrDefaultAsync(od => od.Id == id);

            return orderDetail;
        }


        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails
                .Include(o => o.Order)
                .Include(od => od.Product)
                .ToListAsync();
        }
    }
}
