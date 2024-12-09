using Application.Models;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategory(CreateCategoryRequest categoryRequest);
        Task<CategoryDto> GetCategoryById(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task UpdateCategory(int id, CreateCategoryRequest updateRequest);
        Task DeleteCategory(int id);
    }
}
