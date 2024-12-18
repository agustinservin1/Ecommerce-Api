﻿using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext contex) : base(contex)
        {

            _context = contex;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var order = await _context.Orders
                                      .Include(o => o.User)
                                      .Include(o => o.Details)
                                      .ThenInclude(o => o.Product)
                                      .FirstOrDefaultAsync(o => o.Id == orderId);
            return order;
        }
        public async Task<IEnumerable<Order>> GetAllOrders()
        {

            var orders = await _context.Orders.Include(o => o.User)
                                               .Include(o => o.Details)
                                               .ThenInclude(o => o.Product)
                                               .ToListAsync();
            return orders;
        }

        
        public async Task<bool> Exists(int orderId)
        {
            return await _context.Orders.AnyAsync(o => o.Id == orderId); 
        }
    }
}
