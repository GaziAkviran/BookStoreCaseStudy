using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Services;

namespace PointBooks.Controllers.MvcControllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BookService _bookService;

        public AdminController(BookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Dashboard()
        {
            ViewBag.Title = "Admin Dashboard";
            return View();
        }

        public IActionResult AddBook()
        {
            var model = new Book();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book model)
        {
            if (ModelState.IsValid)
            {
                await _bookService.AddBookAsync(model);
                return RedirectToAction("BookList");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.UpdateBookAsync(book);
                return RedirectToAction("BookList");
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction("BookList");
        }


        public async Task<IActionResult> BookList()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }


    }
}
