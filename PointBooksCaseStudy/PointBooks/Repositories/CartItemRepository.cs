using Dapper;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem>
    {
        public CartItemRepository(DapperContext context) : base(context, "CartItems", "CartItemID", new Dictionary<string, string>
        {
            { "CartID", "Carts" },
            { "BookID", "Books" }
        })
        {

        }

        public async Task<CartItem> GetByCartIdAndBookId(int cartId, int bookId)
        {
            var query = "SELECT * FROM CartItems WHERE CartID = @CartID AND BookID = @BookID";
            using (var connection = Context.CreateConnection())
            {
                var cartItem = await connection.QuerySingleOrDefaultAsync<CartItem>(query, new { CartID = cartId, BookID = bookId });
                return cartItem;
            }
        }

    }
}
