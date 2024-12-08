using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetByIdIncludeCategory(int id);
        Task<IEnumerable<Product>> GetAllProductsWithCategories();
    }
}
