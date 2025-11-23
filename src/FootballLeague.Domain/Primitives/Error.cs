namespace FootballLeague.Domain.Primitives;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public static Error NotFound(string name, Guid id) =>
        new("Error.NotFound", $"{name} with ID {id} was not found.");

    public static Error Validation(string description) =>
        new("Error.Validation", description);
}
