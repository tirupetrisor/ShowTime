using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models;

namespace ShowTime.DataAccess.Configuration;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Image)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(a => a.Festivals)
            .WithMany(f => f.Artists)
            .UsingEntity<Lineup>();

        builder.HasMany(a => a.Lineups)
            .WithOne(l => l.Artist)
            .HasForeignKey(l => l.ArtistId);
    }
}