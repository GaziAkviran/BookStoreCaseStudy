using Dapper;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(DapperContext context) : base(context, "Roles", "RoleID",new Dictionary<string, string>
        {

        })
        {

        } 

        public async Task<Role> GetByNameAsync(string roleName)
        {
            var query = "SELECT * FROM Roles WHERE RoleName = @RoleName";
            using (var connection = Context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Role>(query, new { RoleName = roleName });
            }
        }
    }
}
