namespace FootballLeague.Services.DTOs;

public record MatchOutcome(
    Guid HomeTeamId,
    Guid AwayTeamId,
    int HomeScore,
    int AwayScore);
