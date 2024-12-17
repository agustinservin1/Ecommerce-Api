using Domain.Entities;

namespace Application.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

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
