using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Configuration;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess;

public class ShowTimeDbContext:DbContext
{
    public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
    { }

    public DbSet<Festival> Festivals { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Lineup> Lineups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new ArtistConfiguration().Configure(modelBuilder.Entity<Artist>());
        new FestivalConfiguration().Configure(modelBuilder.Entity<Festival>());
        new LineupConfiguration().Configure(modelBuilder.Entity<Lineup>());
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new BookingConfiguration().Configure(modelBuilder.Entity<Booking>());
    }
}