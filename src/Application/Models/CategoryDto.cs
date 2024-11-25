﻿using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static CategoryDto CreateDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static IEnumerable<CategoryDto> CreateListDto(IEnumerable<Category> categories)
        {
            return categories.Select(CreateDto).ToList();
        }
    }
}
