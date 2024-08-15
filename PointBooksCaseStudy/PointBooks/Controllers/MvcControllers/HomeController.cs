using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Models.ViewModels;
using PointBooks.Repositories;

namespace PointBooks.Controllers.MvcControllers
{
    public class HomeController : Controller
    {
        private readonly GenericRepository<Book> _bookRepository;

        public HomeController(GenericRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAll();
            var cartViewModel = new CartViewModel
            {
                CartItems = new List<CartItem>(),
                TotalPrice = 0
            };

            var viewModel = new HomeIndexViewModel
            {
                Books = books?.ToList() ?? new List<Book>(),
                Cart = cartViewModel
            };

            return View(viewModel);
        }
    }
}
