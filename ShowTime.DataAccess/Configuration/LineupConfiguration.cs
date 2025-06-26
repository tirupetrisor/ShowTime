using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShowTime.DataAccess.Configuration;

public class LineupConfiguration : IEntityTypeConfiguration<Lineup>
{
    public void Configure(EntityTypeBuilder<Lineup> builder)
    {
        builder.ToTable("Lineups");

        builder.HasKey(l => new { l.FestivalId, l.ArtistId });

        builder.Property(l => l.Stage)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.StartTime)
            .IsRequired();

        builder.HasOne(l => l.Festival)
            .WithMany(f => f.Lineups)
            .HasForeignKey(l => l.FestivalId);

        builder.HasOne(l => l.Artist)
            .WithMany(a => a.Lineups)
            .HasForeignKey(l => l.ArtistId);
    }
}
