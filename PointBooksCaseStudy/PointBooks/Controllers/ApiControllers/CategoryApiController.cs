using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryApiController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorys()
        {
            var categories = await _categoryRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            var category = await _categoryRepository.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            await _categoryRepository.Create(category);
            return CreatedAtAction(nameof(GetCategoryByID), new { id = category.CategoryID }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || category.CategoryID != id)
            {
                return BadRequest();
            }

            var existingCategory = await _categoryRepository.GetByID(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryRepository.Update(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryRepository.Delete(id);
            return NoContent();
        }
    }
}
