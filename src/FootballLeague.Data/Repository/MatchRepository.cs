namespace FootballLeague.Data.Repository;

internal sealed class MatchRepository(AppDbContext context) : IMatchRepository
{
    public async Task<Match?> GetByIdWithTeamsAsync(Guid id)
    {
        return await context.Matches
            .AsNoTracking()
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public void Add(Match match)
    {
        context.Matches.Add(match);
    }

    public void Delete(Match match)
    {
        context.Matches.Remove(match);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
