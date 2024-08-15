using Dapper;
using PointBooks.Data;
using PointBooks.Models;

namespace PointBooks.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DapperContext _context;
        private readonly string _tableName;
        private readonly string _primaryKeyName;
        private readonly Dictionary<string, string> _foreignKeys;

        public DapperContext Context => _context;

        public GenericRepository(DapperContext context, string tableName, string primaryKeyName, Dictionary<string, string> foreignKeys)
        {
            _context = context;
            _tableName = tableName;
            _primaryKeyName = primaryKeyName;
            _foreignKeys = foreignKeys;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var query = $"SELECT * FROM {_tableName}";
            using (var connection = _context.CreateConnection())
            {
                var entities = await connection.QueryAsync<T>(query);
                return entities.ToList();
            }
        }

        public async Task<T> GetByID(int id)
        {
            var query = $"SELECT *FROM {_tableName} WHERE {_primaryKeyName} = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            }
        }

        public async Task Create(T entity)
        {
            await ValidateForeignKeys(entity);

            var properties = GetProperties(entity).Where(p => p != _primaryKeyName);
            var query = $"INSERT INTO {_tableName} ({string.Join(", ", properties)}) VALUES ({string.Join(", ", properties.Select(p => $"@{p}"))})";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task Update(T entity)
        {
            await ValidateForeignKeys(entity);

            var properties = GetProperties(entity).Where(p => p != _primaryKeyName);
            var query = $"UPDATE {_tableName} SET {string.Join(", ", properties.Select(p => $"{p} = @{p}"))} WHERE {_primaryKeyName} = @{_primaryKeyName}";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task Delete(int id)
        {
            var query = $"DELETE FROM {_tableName} WHERE {_primaryKeyName} = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }


        private IEnumerable<string> GetProperties(T entity, string prefix = "")
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != _primaryKeyName);
            return properties.Select(p => prefix == "=" ? $"{p.Name} = @{p.Name}" : $"{prefix}{p.Name}");
        }

        public string GetPrimaryKeyName()
        {
            return _primaryKeyName;
        }

        private async Task ValidateForeignKeys(T entity)
        {
            using (var connection = _context.CreateConnection())
            {
                foreach (var foreignKey in _foreignKeys)
                {
                    var property = typeof(T).GetProperty(foreignKey.Key);
                    if (property != null)
                    {
                        var foreignKeyValue = property.GetValue(entity);
                        var query = $"SELECT COUNT(1) FROM {foreignKey.Value} WHERE {foreignKey.Key} = @ForeignKeyValue";
                        var exists = await connection.ExecuteScalarAsync<int>(query, new { ForeignKeyValue = foreignKeyValue });
                        if (exists == 0)
                        {
                            throw new KeyNotFoundException($"{foreignKey.Key} not fount in {foreignKey.Value} table.");
                        }
                    }
                }
            }
        }

        // Book Search
        public async Task<IEnumerable<T>> SearchAsync(string query)
        {
            var tableName = typeof(T).Name + "s";

            var sql = $"SELECT * FROM {tableName} WHERE Title LIKE @Query";

            using (var connection = _context.CreateConnection())
            {
                var results = await connection.QueryAsync<T>(sql, new { Query = $"%{query}%" });
                return results;
            }
        }
    }
}
