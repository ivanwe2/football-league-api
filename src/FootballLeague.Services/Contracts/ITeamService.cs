using FootballLeague.Domain.Primitives;

namespace FootballLeague.Services.Contracts;

public interface ITeamService
{
    Task<Result<List<Team>>> GetRankingAsync(int page, int pageSize);
    Task<Result<Team>> GetByIdAsync(Guid id);
    Task<Result<Team>> CreateAsync(string name);
    Task<Result> UpdateAsync(Guid id, string name);
    Task<Result> DeleteAsync(Guid id);
}
