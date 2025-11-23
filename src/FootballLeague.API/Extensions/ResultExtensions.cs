using FootballLeague.Domain.Primitives;

namespace FootballLeague.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert success result to problem");
        }

        return Results.BadRequest(new
        {
            result.Error.Code,
            result.Error.Description
        });
    }
}
