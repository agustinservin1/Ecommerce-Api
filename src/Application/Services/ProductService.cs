using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService

    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ProductDto> CreateProduct(CreateProductRequest productRequest)
        {
            var category = await _categoryRepository.GetById(productRequest.IdCategory);
            if (category == null)
            {
                throw new NotFoundException($"The category with id {productRequest.IdCategory} that not exist ");
            }
            var product = CreateProductRequest.ToEntity(productRequest, category);
            await _productRepository.Create(product);
            return ProductDto.CreateDto(product);
        }
        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }
            return ProductDto.CreateDto(product);
        }
        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAll();
            if (products == null)
            {
                throw new NotFoundException($"{nameof(GetAllProducts)}");
            }
            return ProductDto.CreateList(products);
        }
        public async Task UpdateProduct(int id, UpdateProductRequest updateRequest)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest), "The product update request cannot be null");
            }
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            var category = await _categoryRepository.GetById(updateRequest.IdCategory);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category), updateRequest.IdCategory);
            }

            UpdateProductRequest.UpdateEntity(product, updateRequest, category);
            await _productRepository.Update(product);
        }
        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }
            await _productRepository.Delete(product);

        }


    }
}
