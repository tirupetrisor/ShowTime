using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Models;

public class Festival
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string SplashArt { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();  // Noua colecție
}


