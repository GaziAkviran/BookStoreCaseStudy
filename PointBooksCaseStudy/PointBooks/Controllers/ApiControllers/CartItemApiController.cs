using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemApiController : ControllerBase
    {
        private readonly IGenericRepository<CartItem> _cartItemRepository;

        public CartItemApiController(IGenericRepository<CartItem> cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _cartItemRepository.GetAll();
            return Ok(cartItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemByID(int id)
        {
            var cartItem = await _cartItemRepository.GetByID(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return Ok(cartItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItem cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest();
            }

            await _cartItemRepository.Create(cartItem);
            return CreatedAtAction(nameof(GetCartItemByID), new { id = cartItem.CartItemID }, cartItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItem cartItem)
        {
            if (cartItem == null || cartItem.CartItemID != id)
            {
                return BadRequest();
            }

            var existingCartItem = await _cartItemRepository.GetByID(id);
            if (existingCartItem == null)
            {
                return NotFound();
            }

            await _cartItemRepository.Update(cartItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _cartItemRepository.GetByID(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            await _cartItemRepository.Delete(id);
            return NoContent();
        }
    }
}
