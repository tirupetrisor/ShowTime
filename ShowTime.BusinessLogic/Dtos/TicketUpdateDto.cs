using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class TicketUpdateDto
{
    [Required(ErrorMessage = "Ticket type is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ticket type must be between 2 and 50 characters")]
    public string Type { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
} 