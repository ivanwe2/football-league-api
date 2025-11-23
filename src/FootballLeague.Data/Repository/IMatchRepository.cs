namespace FootballLeague.Data.Repository;

public interface IMatchRepository
{
    Task<Match?> GetByIdWithTeamsAsync(Guid id);
    void Add(Match match);
    void Delete(Match match);
    Task SaveChangesAsync();
}
