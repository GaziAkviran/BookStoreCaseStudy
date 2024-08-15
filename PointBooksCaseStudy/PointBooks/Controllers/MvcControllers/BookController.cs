using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;
using System.Diagnostics;
using PointBooks.Models.ViewModels;
using PointBooks.Services;
using System.Security.Claims;

namespace PointBooks.Controllers.MvcControllers
{
    public class BookController : Controller
    {

       private readonly BookRepository _bookRepository;
       private readonly BookService _bookService;
       public BookController(BookRepository bookRepository, BookService bookService)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
        }

        [Route("Book/Search")]
        [HttpGet]
        public async Task<IActionResult> Search(string query, string sortBy, List<string> selectedPublishers, List<string> selectedAuthors)
        {
            query = query ?? string.Empty;

            if (string.IsNullOrEmpty(query))
            {
                //return BadRequest("Arama sorgusu boş olamaz.");
            }

            var books = await _bookService.GetBooksByQueryAsync(query);
            
            
            if (selectedPublishers != null && selectedPublishers.Any())
            {
                books = _bookService.GetBooksByPublisher(books.ToList(), selectedPublishers);
            }

            if (selectedAuthors != null && selectedAuthors.Any())
            {
                books = _bookService.GetBooksByAuthor(books.ToList(), selectedAuthors);
            }

            books = _bookService.SortBooks(books, sortBy);
            var publisherCounts = await _bookService.GetPublisherCountsAsync();
            var authorCounts = await _bookService.GetAuthorCountsAsync();
            //var userID = 1;

            if (books == null || !books.Any())
            {
                return View("NoResults");
            }

            var viewModel = new SearchViewModel
            {
                Books = books,
                PublisherCounts = publisherCounts,
                AuthorCounts = authorCounts,
                Query = query,
                //Cart = await _cartService.GetCartByUserId(userID)
            };

            return View("SearchResults", viewModel);
        }

      

        [Route("Book/FilterByPrice")]
        [HttpGet]
        public async Task<IActionResult> FilterByPrice(decimal? minPrice, decimal? maxPrice)
        {
            var books = await _bookRepository.FilterByPriceAsync(minPrice, maxPrice);
            var publisherCounts = await _bookService.GetPublisherCountsAsync();
            var authorCounts = await _bookService.GetAuthorCountsAsync();

            if (books == null || !books.Any())
            {
                return View("NoResults");
            }

            var viewModel = new SearchViewModel
            {
                Books = books,
                PublisherCounts = publisherCounts,
                AuthorCounts = authorCounts,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return View("SearchResults", viewModel);
        }

        public async Task<IActionResult> SearchByCategory(int? categoryID)
        {
            var books = await _bookService.GetBooksByCategoryAsync(categoryID);
            var publisherCounts = await _bookService.GetPublisherCountsAsync();
            var authorCounts = await _bookService.GetAuthorCountsAsync();
           

            var model = new SearchViewModel
            {
                Query = categoryID == 0 ? "Tüm Kitaplar" : categoryID.ToString(),
                Books = books,
                PublisherCounts = publisherCounts,
                AuthorCounts = authorCounts,
            };

            return View("SearchResults", model);
        }
    }
}
