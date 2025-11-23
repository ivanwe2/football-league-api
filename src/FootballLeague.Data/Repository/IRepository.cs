namespace FootballLeague.Data.Repository;

public interface IRepository<T> where T : AuditableEntity
{
    Task<PagedList<T>> GetPagedAsync(
        Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null,
        int page = 1,
        int pageSize = 10);
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
