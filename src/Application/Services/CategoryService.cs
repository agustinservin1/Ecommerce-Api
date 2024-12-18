﻿using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository categoryRepository, ICacheService cacheService, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
            _logger = logger;
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
            string cacheKey = $"category:{id}";

            var cachedCategory = await _cacheService.GetAsync<CategoryDto>(cacheKey);
            if (cachedCategory != null)
            {
                _logger.LogInformation("Recuperando categoria {Id} desde Redis", id);
                return cachedCategory;
            }

            _logger.LogInformation("Recuperando categoría {Id} desde Base de Datos", id);
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                _logger.LogWarning("Categoría {Id} no encontrada", id);
                throw new NotFoundException(nameof(Category), id);
            }

            var categoryDto = CategoryDto.CreateDto(category);

            await _cacheService.SetAsync(cacheKey, categoryDto, TimeSpan.FromMinutes(15));
            _logger.LogInformation("Categoría {Id} almacenada en caché", id);

            return categoryDto;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            string cacheKey = "categories:all";

            var cachedCategories = await _cacheService.GetAsync<IEnumerable<CategoryDto>>(cacheKey);
            if (cachedCategories != null)
            {
                return cachedCategories;
            }

            var categories = await _categoryRepository.GetAll();
            if (categories == null || !categories.Any())
            {
                throw new NotFoundException($"{nameof(GetAllCategories)}");
            }

            var categoryDtos = CategoryDto.CreateListDto(categories);

            await _cacheService.SetAsync(cacheKey, categoryDtos, TimeSpan.FromMinutes(15));

            return categoryDtos;
        }
        public async Task UpdateCategory(int id, CreateCategoryRequest updateRequest)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new NotFoundException();
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



