using FootballLeague.API.Extensions;
using FootballLeague.Services.Contracts;
using FootballLeague.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.API.Endpoints;

public record CreateMatchRequest(Guid HomeTeamId, Guid AwayTeamId, int HomeScore, int AwayScore);
public record UpdateScoreRequest(int HomeScore, int AwayScore);

public static class EndpointExtensions
{
    public static void MapTeamEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/teams").WithTags("Teams");

        group.MapGet("/ranking", async (ITeamService service, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
        {
            var result = await service.GetRankingAsync(page, pageSize);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        group.MapPost("/", async ([FromBody] string name, ITeamService service) =>
        {
            var result = await service.CreateAsync(name);
            return result.IsSuccess ? Results.Created($"/teams/{result.Value.Id}", result.Value) : result.ToProblemDetails();
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] string name, ITeamService service) =>
        {
            var result = await service.UpdateAsync(id, name);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ITeamService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        });
    }

    public static void MapMatchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/matches").WithTags("Matches");

        group.MapPost("/", async ([FromBody] CreateMatchRequest req, IMatchService service) =>
        {
            var outcome = new MatchOutcome(req.HomeTeamId, req.AwayTeamId, req.HomeScore, req.AwayScore);
            var result = await service.RegisterMatchAsync(outcome);
            return result.IsSuccess ? Results.Created($"/matches/{result.Value}", new { Id = result.Value }) : result.ToProblemDetails();
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateScoreRequest req, IMatchService service) =>
        {
            var result = await service.UpdateScoreAsync(id, req.HomeScore, req.AwayScore);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IMatchService service) =>
        {
            var result = await service.DeleteMatchAsync(id);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        });
    }
}
