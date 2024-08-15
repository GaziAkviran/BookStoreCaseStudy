using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(DapperContext context) : base(context, "Categories", "CategoryID", new Dictionary<string, string>
        {

        })
        {

        }
    }
}
