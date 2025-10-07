using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class BookingCreateDto
{
    [Required(ErrorMessage = "Festival ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Festival ID must be a positive number")]
    public int FestivalId { get; set; }
    
    [Required(ErrorMessage = "User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Ticket ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Ticket ID must be a positive number")]
    public int TicketId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
    public int Quantity { get; set; } = 1;
} 