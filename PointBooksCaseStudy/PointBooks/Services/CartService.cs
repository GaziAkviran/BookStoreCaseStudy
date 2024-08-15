using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly CartItemRepository _cartItemRepository;

        public CartService(CartRepository cartRepository, CartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _cartRepository.GetCartByUserId(userId);
        }

        public async Task<Cart> CreateOrUpdateCart(int userId, CartItem cartItem)
        {
            return await _cartRepository.CreateOrUpdateCart(userId, cartItem);
        }

        public async Task UpdateCartItemQuantity(int cartId, int bookId, int quantity)
        {
            await _cartRepository.UpdateCartItemQuantity(cartId, bookId, quantity);
        }
    }
}
