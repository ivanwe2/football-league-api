using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballLeague.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Points).HasDefaultValue(0);
        builder.Property(t => t.Wins).HasDefaultValue(0);
        builder.Property(t => t.Draws).HasDefaultValue(0);
        builder.Property(t => t.Losses).HasDefaultValue(0);
    }
}
