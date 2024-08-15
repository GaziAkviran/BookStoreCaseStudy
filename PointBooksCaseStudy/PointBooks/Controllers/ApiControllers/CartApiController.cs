using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;
using PointBooks.Services;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly CartService _cartService;

        public CartApiController(IGenericRepository<Cart> cartRepository, CartService cartService)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
        }

        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound("Kullanıcıya ait sepet bulunamadı.");
            }
            return Ok(cart);
        }

        [HttpPost("CreateOrUpdateCart")]
        public async Task<IActionResult> CreateOrUpdateCart(int userId, [FromBody] CartItem cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Sepet öğesi geçersiz.");
            }

            var cart = await _cartService.CreateOrUpdateCart(userId, cartItem);
            return Ok(cart);
        }

        [HttpPost("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItem(int cartId, int bookId, int quantity)
        {
            await _cartService.UpdateCartItemQuantity(cartId, bookId, quantity);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var authors = await _cartRepository.GetAll();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartByID(int id)
        {
            var cart = await _cartRepository.GetByID(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] Cart cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }

            await _cartRepository.Create(cart);
            return CreatedAtAction(nameof(GetCartByID), new { id = cart.CartID }, cart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] Cart cart)
        {
            if (cart == null || cart.CartID != id)
            {
                return BadRequest();
            }

            var existingCart = await _cartRepository.GetByID(id);
            if (existingCart == null)
            {
                return NotFound();
            }

            await _cartRepository.Update(cart);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _cartRepository.GetByID(id);
            if (cart == null)
            {
                return NotFound();
            }

            await _cartRepository.Delete(id);
            return NoContent();
        }
    }
}
