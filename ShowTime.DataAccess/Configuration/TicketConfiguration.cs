using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Configuration;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Price)
            .IsRequired();

        builder.HasOne(t => t.Festival)
            .WithMany(f => f.Tickets)
            .HasForeignKey(t => t.FestivalId);
    }
} 