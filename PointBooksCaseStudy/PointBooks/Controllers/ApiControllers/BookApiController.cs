using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepository;

        public BookApiController(IGenericRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByID(int id)
        {
            var book = await _bookRepository.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBookByID), new { id = book.BookID }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookBy(int id, [FromBody] Book book)
        {
            if (book == null || book.BookID != id)
            {
                return BadRequest();
            }

            var existingBook = await _bookRepository.GetByID(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            await _bookRepository.Update(book);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookRepository.Delete(id);
            return NoContent();
        }
    }

}
