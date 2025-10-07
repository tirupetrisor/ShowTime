using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class TicketCreateDto
{
    [Required(ErrorMessage = "Festival ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Festival ID must be a positive number")]
    public int FestivalId { get; set; }
    
    [Required(ErrorMessage = "Ticket type is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ticket type must be between 2 and 50 characters")]
    public string Type { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 1000000, ErrorMessage = "Capacity must be between 1 and 1,000,000")]
    public int Capacity { get; set; }
} 