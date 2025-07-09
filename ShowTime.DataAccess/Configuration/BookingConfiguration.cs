using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Configuration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.Festival)
            .WithMany(f => f.Bookings)
            .HasForeignKey(b => b.FestivalId);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        builder.HasOne(b => b.Ticket)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TicketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
