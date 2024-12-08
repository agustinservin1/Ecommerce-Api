using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
