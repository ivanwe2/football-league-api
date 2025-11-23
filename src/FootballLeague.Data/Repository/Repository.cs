using FootballLeague.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Data.Repository;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : AuditableEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<PagedList<T>> GetPagedAsync(
        Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null,
        int page = 1,
        int pageSize = 10)
    {
        IQueryable<T> query = _dbSet;

        if (queryBuilder != null)
        {
            query = queryBuilder(query);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PagedList<T>.Create(items, page, pageSize, totalCount);
    }
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOnUtc = DateTime.Now;

            _dbSet.Update(entity);
        }
    }
}
