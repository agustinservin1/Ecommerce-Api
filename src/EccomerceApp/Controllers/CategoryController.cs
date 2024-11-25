using Application.Interfaces;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Category data is null"); 
            }
            var createdCategory = await _categoryService.CreateCategory(request);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody]CreateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Category data is null");
            }

            await _categoryService.UpdateCategory(id, request);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }

    }
}
