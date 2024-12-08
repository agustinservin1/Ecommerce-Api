using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public static Category ToEntity(CreateCategoryRequest dto)
        {
            return new Category
            {
                Name = dto.Name
            };
        }
    }   
}


