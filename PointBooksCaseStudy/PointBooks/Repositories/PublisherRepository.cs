using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>
    {
        public PublisherRepository(DapperContext context) : base (context, "Publishers", "PublisherID", new Dictionary<string, string>
        {

        })
        {

        }
    }
}
