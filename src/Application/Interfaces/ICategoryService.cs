using Application.Dtos;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
