using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Configurations;
using ShowTime.DataAccess.Models;

namespace ShowTime.DataAccess;

public class ShowTimeContext : DbContext
{
    public ShowTimeContext(DbContextOptions<ShowTimeContext> options)
        : base(options)
    {
    }

    public DbSet<Festival> Festivals { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Lineup> Lineups { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

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