using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> CreateCategory(CreateCategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryRequest), "The category request cannot be null");
            }
            var category = CreateCategoryRequest.ToEntity(categoryRequest);
            await _categoryRepository.Create(category);
            return CategoryDto.CreateDto(category);

        }
        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }
            return CategoryDto.CreateDto(category);
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll();
            if (categories == null)
            {
                throw new NotFoundException($"{nameof(GetAllCategories)}");
            }
            return CategoryDto.CreateListDto(categories);
        }
        public async Task UpdateCategory(int id, CreateCategoryRequest updateRequest)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new NotFoundException($"Unable to create category {id}");
            }

            category.Name = updateRequest.Name;
            await _categoryRepository.Update(category);
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }
            await _categoryRepository.Delete(category);

        }
    }
}



