using FootballLeague.Domain.Models;

namespace FootballLeague.Services.Strategies;

public interface IScoringStrategy
{
    void Calculate(Team home, Team away, int homeScore, int awayScore);
    void RevertPoints(Team home, Team away, int homeScore, int awayScore);
}