using Dapper;
using Microsoft.Data.SqlClient;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class BookRepository : GenericRepository<Book>
    {
        public BookRepository(DapperContext context) : base(context, "Books", "BookID", new Dictionary<string, string>
        {
            { "CategoryID", "Categories" },
            { "PublisherID", "Publishers" }
        })
        {

        }

        public async Task<IEnumerable<PublisherCount>> GetPublisherCountsAsync()
        {
            var sql = @"
                SELECT 
                    p.PublisherName, 
                    COUNT(b.BookID) AS BookCount
                FROM 
                    Books b
                INNER JOIN 
                    Publishers p ON b.PublisherID = p.PublisherID
                GROUP BY 
                    p.PublisherName";

            using (var connection = Context.CreateConnection())
            {
                var publisherCounts = await connection.QueryAsync<PublisherCount>(sql);
                return publisherCounts.ToList();
            }
        }

        public async Task<IEnumerable<AuthorCount>> GetAuthorCountsAsync()
        {
            var sql = @"
                SELECT 
                    a.Name + ' ' + a.Surname AS AuthorName, 
                    COUNT(ba.BookID) AS BookCount
                FROM 
                    BookAuthors ba
                INNER JOIN 
                    Authors a ON ba.AuthorID = a.AuthorID
                GROUP BY 
                    a.Name, a.Surname";

            using (var connection = Context.CreateConnection())
            {
                var authorCounts = await connection.QueryAsync<AuthorCount>(sql);
                return authorCounts;
            }
        }

        public async Task<Dictionary<string, int>> GetBookCountByPriceRangeAsync()
        {
            var sql = @"
                SELECT 
                    CASE 
                        WHEN Price BETWEEN 0 AND 50 THEN '0-50'
                        WHEN Price BETWEEN 50 AND 100 THEN '50-100'
                        WHEN Price BETWEEN 100 AND 150 THEN '100-150'
                        WHEN Price BETWEEN 150 AND 200 THEN '150-200'
                        WHEN Price BETWEEN 200 AND 250 THEN '200-250'
                    END AS PriceRange, 
                    COUNT(*) AS BookCount
                FROM Books
                GROUP BY 
                    CASE 
                        WHEN Price BETWEEN 0 AND 50 THEN '0-50'
                        WHEN Price BETWEEN 50 AND 100 THEN '50-100'
                        WHEN Price BETWEEN 100 AND 150 THEN '100-150'
                        WHEN Price BETWEEN 150 AND 200 THEN '150-200'
                        WHEN Price BETWEEN 200 AND 250 THEN '200-250'
                    END";

            using (var connection = Context.CreateConnection())
            {
                var result = await connection.QueryAsync(sql);
                return result.ToDictionary(row => (string)row.PriceRange, row => (int)row.BookCount);
            }
        }

        public async Task<IEnumerable<Book>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            var sql = "SELECT * FROM Books WHERE Price BETWEEN @MinPrice AND @MaxPrice";

            using (var connection = Context.CreateConnection())
            {
                var books = await connection.QueryAsync<Book>(sql, new { MinPrice = minPrice ?? 0, MaxPrice = maxPrice ?? decimal.MaxValue });
                return books;
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryID)
        {
            var query = @"
            SELECT b.*
            FROM Books b
            INNER JOIN Categories c ON b.CategoryID = c.CategoryID
            WHERE b.CategoryID = @CategoryID";

            using (var connection = Context.CreateConnection())
            {
                return await connection.QueryAsync<Book>(query, new { CategoryID = categoryID });
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var query = "SELECT * FROM Books";

            using (var connection = Context.CreateConnection())
            {
                return await connection.QueryAsync<Book>(query);
            }
        }

        public async Task AddBookAsync(Book book)
        {
            await Create(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await Update(book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            await Delete(bookId);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await GetByID(bookId);
        }
    }
}
