using Dapper;
using PointBooks.Data;
using PointBooks.Models;
using System.Net;

namespace PointBooks.Repositories
{
    public class BookAuthorRepository : GenericRepository<BookAuthor>
    {
        private readonly DapperContext _context;

        public BookAuthorRepository(DapperContext context) : base(context, "BookAuthors", "", new Dictionary<string, string>
        {
            { "BookID", "Books" },
            { "AuthorID", "Authors" }
        })
        {
            _context = context;
        }

        public async Task<BookAuthor> GetByID(int bookID, int authorID)
        {
            var query = "SELECT * FROM BookAuthors WHERE BookID = BookID AND AuthorID = AuthorID";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BookAuthor>(query, new { BookID = bookID, AuthorID = authorID });

            }
        }

        public async Task Delete(int bookID, int authorID)
        {
            var query = "DELETE FROM BookAuthors WHERE BookID = @BookID AND AuthorID = @AuthorID";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { BookID = bookID, AuthorID = authorID });
            }
        }


    }
}
