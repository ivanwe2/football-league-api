using FootballLeague.Domain.Base;
using FootballLeague.Domain.Primitives;

namespace FootballLeague.Domain.Models;

public class Team : AuditableEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Points { get; private set; }
    public int Wins { get; private set; }
    public int Draws { get; private set; }
    public int Losses { get; private set; }

    private Team() { }

    public static Result<Team> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Team>(Error.Validation("Name cannot be empty."));

        return Result.Success(new Team
        {
            Id = Guid.NewGuid(),
            Name = name,
        });
    }

    public void UpdateName(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName))
            Name = newName;
    }

    public void AddWin() { Wins++; Points += 3; }
    public void AddDraw() { Draws++; Points += 1; }
    public void AddLoss() { Losses++; }

    public void RemoveWin() { Wins--; Points -= 3; }
    public void RemoveDraw() { Draws--; Points -= 1; }
    public void RemoveLoss() { Losses--; }
}
