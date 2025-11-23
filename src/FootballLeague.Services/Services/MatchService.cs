using FootballLeague.Data.Repository;
using FootballLeague.Domain.Primitives;
using FootballLeague.Services.Contracts;
using FootballLeague.Services.DTOs;
using FootballLeague.Services.Strategies;

namespace FootballLeague.Services.Services;

public class MatchService(
    IMatchRepository matchRepo,
    ITeamRepository teamRepo,
    IScoringStrategy strategy)
    : IMatchService
{
    public async Task<Result<Guid>> RegisterMatchAsync(MatchOutcome outcome)
    {
        var teams = await teamRepo.GetManyByIdsAsync([outcome.HomeTeamId, outcome.AwayTeamId]);

        if (teams.Count != 2)
        {
            return Result.Failure<Guid>(Error.NotFound("Team", Guid.Empty));
        }

        var home = teams.First(t => t.Id == outcome.HomeTeamId);
        var away = teams.First(t => t.Id == outcome.AwayTeamId);

        var matchResult = Match.Create(outcome.HomeTeamId, outcome.AwayTeamId, outcome.HomeScore, outcome.AwayScore);

        if (matchResult.IsFailure)
        {
            return Result.Failure<Guid>(matchResult.Error);
        }

        strategy.Calculate(home, away, outcome.HomeScore, outcome.AwayScore);

        matchRepo.Add(matchResult.Value);
        await matchRepo.SaveChangesAsync();

        return Result.Success(matchResult.Value.Id);
    }

    public async Task<Result> UpdateScoreAsync(Guid matchId, int newHomeScore, int newAwayScore)
    {
        var match = await matchRepo.GetByIdWithTeamsAsync(matchId);

        if (match is null)
        {
            return Result.Failure(Error.NotFound("Match", matchId));
        }

        strategy.RevertPoints(match.HomeTeam, match.AwayTeam, match.HomeScore, match.AwayScore);
        strategy.Calculate(match.HomeTeam, match.AwayTeam, newHomeScore, newAwayScore);

        match.UpdateScore(newHomeScore, newAwayScore);

        await matchRepo.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteMatchAsync(Guid matchId)
    {
        var match = await matchRepo.GetByIdWithTeamsAsync(matchId);

        if (match is null)
        {
            return Result.Failure(Error.NotFound("Match", matchId));
        }

        strategy.RevertPoints(match.HomeTeam, match.AwayTeam, match.HomeScore, match.AwayScore);

        matchRepo.Delete(match);
        await matchRepo.SaveChangesAsync();
        return Result.Success();
    }
}
