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
        // Método para crear un nuevo detalle de orden
        public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail); // Agregar el detalle
            await _context.SaveChangesAsync(); // Guardar cambios
            return orderDetail; // Retornar el detalle creado
        }

        // Método para crear múltiples detalles de orden
        public async Task CreateOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            await _context.OrderDetails.AddRangeAsync(orderDetails); // Agregar todos los detalles
            await _context.SaveChangesAsync(); // Guardar cambios
        }


        // Método para obtener todos los detalles de la base de datos
        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails
                .Include(od => od.Product) // Incluir los productos asociados
                .ToListAsync(); // Obtener todos los detalles
        }
    }
}
