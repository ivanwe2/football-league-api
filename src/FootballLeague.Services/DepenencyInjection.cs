using FootballLeague.Services.Contracts;
using FootballLeague.Services.Services;
using FootballLeague.Services.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FootballLeague.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<IScoringStrategy, StandardScoringStrategy>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IMatchService, MatchService>();

        return services;
    }
}
