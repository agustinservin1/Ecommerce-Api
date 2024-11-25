using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


