using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Models;

public class Booking
{
    public int Id { get; set; }  // Noua cheie primară
    public int FestivalId { get; set; }
    public int UserId { get; set; }
    public int TicketId { get; set; }  // FK către Ticket
    
    // Navigation properties
    public Festival Festival { get; set; } = null!;
    public User User { get; set; } = null!;
    public Ticket Ticket { get; set; } = null!;  // Noua relație
}