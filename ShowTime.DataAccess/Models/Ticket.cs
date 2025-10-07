using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Models;

public class Ticket
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Capacity { get; set; }
    
    // Navigation properties
    public Festival Festival { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
} 