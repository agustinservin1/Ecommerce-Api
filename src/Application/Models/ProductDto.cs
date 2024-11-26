using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Application.Dtos;

namespace Application.Models
{

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public CategoryDto Category { get; set; } = new CategoryDto();
        public string Status { get; set; } = string.Empty;

        public static ProductDto CreateDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                Image = product.Image,
                Category = CategoryDto.CreateDto(product.Category),
                Status = product.Status.ToString()
            };
        }

        public static IEnumerable<ProductDto> CreateList(IEnumerable<Product> products)
        {
            return products.Select(CreateDto).ToList();
        }
    }
}
