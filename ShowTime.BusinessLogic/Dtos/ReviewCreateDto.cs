using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class ReviewCreateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int FestivalId { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [StringLength(2000)]
    public string Comment { get; set; } = string.Empty;
}


