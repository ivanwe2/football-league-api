using FootballLeague.Data.Repository;
using FootballLeague.Domain.Primitives;
using FootballLeague.Services.Contracts;

namespace FootballLeague.Services.Services;

internal sealed class TeamService(ITeamRepository repo)
    : ITeamService
{
    public async Task<Result<List<Team>>> GetRankingAsync(int page, int pageSize)
    {
        var teams = await repo.GetRankingAsync(page, pageSize);
        return Result.Success(teams);
    }

    public async Task<Result<Team>> GetByIdAsync(Guid id)
    {
        var team = await repo.GetByIdAsync(id);
        if (team is null)
        {
            return Result.Failure<Team>(Error.NotFound("Team", id));
        }
        return Result.Success(team);
    }

    public async Task<Result<Team>> CreateAsync(string name)
    {
        var teamResult = Team.Create(name);
        if (teamResult.IsFailure)
        {
            return teamResult;
        }

        repo.Add(teamResult.Value);
        await repo.SaveChangesAsync();
        return Result.Success(teamResult.Value);
    }

    public async Task<Result> UpdateAsync(Guid id, string name)
    {
        var team = await repo.GetByIdAsync(id);
        if (team is null)
        {
            return Result.Failure(Error.NotFound("Team", id));
        }

        team.UpdateName(name);
        await repo.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var team = await repo.GetByIdAsync(id);
        if (team is null)
        {
            return Result.Failure(Error.NotFound("Team", id));
        }

        repo.Delete(team);
        await repo.SaveChangesAsync();
        return Result.Success();
    }
}
