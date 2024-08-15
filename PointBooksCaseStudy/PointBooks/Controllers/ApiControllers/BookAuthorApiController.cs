using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Repositories;
using PointBooks.Models;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorApiController : ControllerBase
    {
        private readonly BookAuthorRepository _bookAuthorRepository;

        public BookAuthorApiController(BookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookAuthors()
        {
            var bookAuthors = await _bookAuthorRepository.GetAll();
            return Ok(bookAuthors);
        }

        [HttpGet("{bookID}/{authorID}")]
        public async Task<IActionResult> GetBookAuthorByID(int bookID, int authorID)
        {
            var bookAuthor = await _bookAuthorRepository.GetByID(bookID, authorID);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return Ok(bookAuthor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookAuthor bookAuthor)
        {
            if (bookAuthor == null)
            {
                return BadRequest();
            }

            await _bookAuthorRepository.Create(bookAuthor);
            return CreatedAtAction(nameof(GetBookAuthorByID), new { bookID = bookAuthor.BookID, authorID = bookAuthor.AuthorID }, bookAuthor);
        }

        [HttpPut("{bookID}/{authorID}")]
        public async Task<IActionResult> Update(int bookID, int authorID, [FromBody] BookAuthor bookAuthor)
        {
            if (bookAuthor == null || bookAuthor.BookID != bookID || bookAuthor.AuthorID != authorID)
            {
                return BadRequest();
            }
            var existingBookAuthor = await _bookAuthorRepository.GetByID(bookID, authorID);
            if (existingBookAuthor == null)
            {
                return NotFound();
            }
            await _bookAuthorRepository.Update(bookAuthor);
            return NoContent();
        }

        [HttpDelete("{bookID}/{authorID}")]
        public async Task<IActionResult> Delete(int bookID, int authorID)
        {
            var bookAuthor = await _bookAuthorRepository.GetByID(bookID, authorID);
            if (bookAuthor == null)
            {
                return NotFound();
            }
            await _bookAuthorRepository.Delete(bookID, authorID);
            return NoContent();
        }

    }
}
