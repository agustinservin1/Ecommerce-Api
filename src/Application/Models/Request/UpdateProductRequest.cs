using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class UpdateProductRequest
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

            public static void UpdateEntity(Product product, UpdateProductRequest dto, Category category)
            {
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Stock = dto.Stock;
                product.Price = dto.Price;
                product.Image = dto.Image;
                product.Category = category;
            }
        }
    }

