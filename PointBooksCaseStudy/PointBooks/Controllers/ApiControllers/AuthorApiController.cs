using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorApiController : ControllerBase
    {
        private readonly IGenericRepository<Author> _authorRepository;

        public AuthorApiController(IGenericRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAll();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorByID(int id)
        {
            var author = await _authorRepository.GetByID(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            await _authorRepository.Create(author);
            return CreatedAtAction(nameof(GetAuthorByID), new { id = author.AuthorID }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
        {
            if (author == null || author.AuthorID != id)
            {
                return BadRequest();
            }

            var existingAuthor = await _authorRepository.GetByID(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            await _authorRepository.Update(author);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByID(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorRepository.Delete(id);
            return NoContent();
        }
    }
}
