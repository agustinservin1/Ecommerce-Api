using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateProductRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Stock { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public int IdCategory { get; set; }

        public static Product ToEntity(CreateProductRequest dto, Category category)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Stock = dto.Stock,
                Price = dto.Price,
                Image = dto.Image,
                Category = category,
            };
        }
}
}

