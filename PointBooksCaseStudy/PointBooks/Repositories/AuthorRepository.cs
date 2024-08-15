using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(DapperContext context) : base(context, "Authors", "AuthorID", new Dictionary<string, string>
        {

        }) 
        {
        
        }
    }
}
