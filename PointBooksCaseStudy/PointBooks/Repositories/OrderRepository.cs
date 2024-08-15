using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(DapperContext context) : base(context, "Orders", "OrderID", new Dictionary<string, string>
        {
            { "UserID", "Users" },
            { "CartID", "Carts" }
        })
        {

        }
    }
}
