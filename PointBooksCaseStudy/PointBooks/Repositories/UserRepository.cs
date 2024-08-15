using Dapper;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DapperContext context) : base (context, "Users", "UserID", new Dictionary<string, string>
        {
            { "RoleID", "Roles" }
        })
        {

        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            using (var connection = Context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
            }
        }
    }
}
