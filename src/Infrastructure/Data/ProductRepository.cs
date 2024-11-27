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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;

        }
        public async Task<Product> GetByIdIncludeCategory(int id)
        {
            var product = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
        public async Task<IEnumerable<Product>> GetAllProductsWithCategories()
        {
            var products = await _context.Products.Include(c => c.Category).ToListAsync();
            return products;
        }
    }
}
