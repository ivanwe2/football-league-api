namespace FootballLeague.Data.Repository;

internal sealed class TeamRepository(AppDbContext context) : ITeamRepository
{
    public async Task<Team?> GetByIdAsync(Guid id)
    {
        return await context.Teams
            .FindAsync(id);
    }

    public async Task<List<Team>> GetManyByIdsAsync(List<Guid> ids)
    {
        return await context.Teams
            .AsNoTracking()
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();
    }

    public void Add(Team team)
    {
        context.Teams.Add(team);
    }

    public void Delete(Team team)
    {
        context.Teams.Remove(team);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<List<Team>> GetAllAsync(int page, int pageSize)
    {
        return await context.Teams.AsNoTracking()
            .OrderBy(t => t.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Team>> GetRankingAsync(int page, int pageSize)
    {
        return await context.Teams.AsNoTracking()
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.Wins)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}