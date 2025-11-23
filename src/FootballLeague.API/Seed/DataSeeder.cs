using FootballLeague.Data;
using FootballLeague.Domain.Models;
using FootballLeague.Services.Strategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace FootballLeague.API.Seed;

public class DataSeeder(
    AppDbContext context,
    IFeatureManager featureManager,
    IScoringStrategy scoringStrategy)
{
    public async Task SeedAsync()
    {
        if (!await featureManager.IsEnabledAsync("EnableSeeding"))
        {
            return;
        }

        if (await context.Teams.AnyAsync())
        {
            return;
        }

        var teams = GetTeams();
        context.Teams.AddRange(teams);
        await context.SaveChangesAsync();

        var matches = GetMatches(teams);
        context.Matches.AddRange(matches);
        await context.SaveChangesAsync();
    }

    private static List<Team> GetTeams()
    {
        var names = new[]
        {
            "Arsenal", "Aston Villa", "Bournemouth", "Brentford",
            "Brighton", "Chelsea", "Crystal Palace", "Everton",
            "Fulham", "Liverpool", "Luton Town", "Man City",
            "Man Utd", "Newcastle", "Nottm Forest", "Sheffield Utd",
            "Tottenham", "West Ham", "Wolves"
        };

        var teams = new List<Team>();

        foreach (var name in names)
        {
            var result = Team.Create(name);
            if (result.IsSuccess)
            {
                teams.Add(result.Value);
            }
        }

        return teams;
    }

    private List<Match> GetMatches(List<Team> teams)
    {
        var matches = new List<Match>();
        var random = new Random();
        int matchesToGenerate = 20;

        for (int i = 0; i < matchesToGenerate; i++)
        {
            var home = teams[random.Next(teams.Count)];
            var away = teams[random.Next(teams.Count)];

            while (home.Id == away.Id)
            {
                away = teams[random.Next(teams.Count)];
            }

            int homeScore = random.Next(0, 5);
            int awayScore = random.Next(0, 5);

            var matchResult = Match.Create(home.Id, away.Id, homeScore, awayScore);

            if (matchResult.IsSuccess)
            {
                var match = matchResult.Value;
                scoringStrategy.Calculate(home, away, homeScore, awayScore);
                matches.Add(match);
            }
        }

        return matches;
    }
}
