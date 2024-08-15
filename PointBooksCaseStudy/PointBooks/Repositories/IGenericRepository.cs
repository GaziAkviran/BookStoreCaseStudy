namespace PointBooks.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
        string GetPrimaryKeyName();

        Task<IEnumerable<T>> SearchAsync(string query);
    }
}
