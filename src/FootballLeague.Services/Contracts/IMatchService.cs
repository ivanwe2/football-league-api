using FootballLeague.Domain.Primitives;
using FootballLeague.Services.DTOs;

namespace FootballLeague.Services.Contracts;

public interface IMatchService
{
    Task<Result<Guid>> RegisterMatchAsync(MatchOutcome outcome);
    Task<Result> UpdateScoreAsync(Guid matchId, int newHomeScore, int newAwayScore);
    Task<Result> DeleteMatchAsync(Guid matchId);
}
