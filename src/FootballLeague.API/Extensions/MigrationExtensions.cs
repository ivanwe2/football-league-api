using FootballLeague.API.Seed;
using FootballLeague.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var dbContext = services.GetRequiredService<AppDbContext>();
        var seeder = services.GetRequiredService<DataSeeder>();

        await dbContext.Database.MigrateAsync();

        await seeder.SeedAsync();
    }
}
