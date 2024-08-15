using Dapper;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository(DapperContext context) : base(context, "Carts", "CartID",new Dictionary<string, string>
        {
            { "UserID", "Users" },
        })
        {

        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            var query = "SELECT * FROM Carts WHERE UserID = @UserID";
            using (var connection = Context.CreateConnection())
            {
                var cart = await connection.QuerySingleOrDefaultAsync<Cart>(query, new { UserID = userId });
                return cart;
            }
        }

        public async Task<Cart> CreateOrUpdateCart(int userId, CartItem cartItem)
        {
            var existingCart = await GetCartByUserId(userId);

            if (existingCart == null)
            {
                var newCart = new Cart
                {
                    UserID = userId,
                    //CreatedDate = DateTime.Now
                };

                await Create(newCart);

                existingCart = await GetCartByUserId(userId);
            }

         
            cartItem.CartID = existingCart.CartID;
            var cartItemRepo = new CartItemRepository(Context);
            var existingCartItem = await cartItemRepo.GetByCartIdAndBookId(existingCart.CartID, cartItem.BookID);

            if (existingCartItem == null)
            {
                await cartItemRepo.Create(cartItem);
            }
            else
            {
                existingCartItem.Quantity += cartItem.Quantity;
                await cartItemRepo.Update(existingCartItem);
            }

            return existingCart;
        }

        public async Task UpdateCartItemQuantity(int cartId, int bookId, int quantity)
        {
            var cartItemRepo = new CartItemRepository(Context);
            var existingCartItem = await cartItemRepo.GetByCartIdAndBookId(cartId, bookId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity = quantity;

                if (existingCartItem.Quantity <= 0)
                {
                    await cartItemRepo.Delete(existingCartItem.CartItemID);
                }
                else
                {
                    await cartItemRepo.Update(existingCartItem);
                }
            }
        }

        public async Task<decimal> GetTotalPrice(int cartId)
        {
            var query = @"
            SELECT SUM(b.Price * ci.Quantity)
            FROM CartItems ci
            INNER JOIN Books b ON ci.BookID = b.BookID
            WHERE ci.CartID = @CartID";

            using (var connection = Context.CreateConnection())
            {
                var total = await connection.ExecuteScalarAsync<decimal>(query, new { CartID = cartId });
                return total;
            }
        }
    }
}
