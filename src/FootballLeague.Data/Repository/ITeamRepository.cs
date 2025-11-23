namespace FootballLeague.Data.Repository;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<List<Team>> GetManyByIdsAsync(List<Guid> ids);
    Task<List<Team>> GetAllAsync(int page, int pageSize);
    Task<List<Team>> GetRankingAsync(int page, int pageSize);
    void Add(Team team);
    void Delete(Team team);
    Task SaveChangesAsync();
}
