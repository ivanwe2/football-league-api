using FootballLeague.Domain.Base;
using FootballLeague.Domain.Primitives;

namespace FootballLeague.Domain.Models;

public class Match : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid HomeTeamId { get; private set; }
    public Team HomeTeam { get; private set; } = null!;
    public Guid AwayTeamId { get; private set; }
    public Team AwayTeam { get; private set; } = null!;
    public int HomeScore { get; private set; }
    public int AwayScore { get; private set; }
    public DateTime MatchDate { get; private set; }

    private Match() { }

    public static Result<Match> Create(Guid homeTeamId, Guid awayTeamId, int homeScore, int awayScore)
    {
        if (homeTeamId == awayTeamId)
            return Result.Failure<Match>(Error.Validation("A team cannot play against itself."));

        if (homeScore < 0 || awayScore < 0)
            return Result.Failure<Match>(Error.Validation("Scores cannot be negative."));

        return Result.Success(new Match
        {
            Id = Guid.NewGuid(),
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            HomeScore = homeScore,
            AwayScore = awayScore,
            MatchDate = DateTime.UtcNow
        });
    }

    public void UpdateScore(int homeScore, int awayScore)
    {
        HomeScore = homeScore;
        AwayScore = awayScore;
    }
}
