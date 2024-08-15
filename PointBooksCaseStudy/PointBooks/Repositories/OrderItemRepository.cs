using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        public OrderItemRepository(DapperContext context) : base(context, "OrderItems", "OrderItemID", new Dictionary<string, string>
        {
            { "OrderID", "Ordes" },
            { "BookID", "Books" }
        })
        {

        }
    }
}
