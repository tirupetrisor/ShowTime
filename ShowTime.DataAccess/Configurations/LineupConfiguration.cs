using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShowTime.DataAccess.Models;

namespace ShowTime.DataAccess.Configurations;

public class LineupConfiguration : IEntityTypeConfiguration<Lineup>
{
    public void Configure(EntityTypeBuilder<Lineup> builder)
    {
        builder.ToTable("Lineups");
        builder.HasKey(l => new { l.FestivalId, l.ArtistId });
        builder.Property(l => l.Stage)
            .IsRequired()
            .HasMaxLength(100);
    }
}