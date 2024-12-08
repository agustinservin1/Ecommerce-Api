using Application.Models;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateProduct(CreateProductRequest productRequest);

        Task<ProductDto> GetProductById(int id);
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task UpdateProduct(int id, UpdateProductRequest updateRequest);
        Task DeleteProduct(int id);




    }
}
