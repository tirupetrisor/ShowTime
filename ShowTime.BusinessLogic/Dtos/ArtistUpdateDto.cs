using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class ArtistUpdateDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Artist name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Artist name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string Image { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Genre is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Genre must be between 2 and 50 characters")]
    public string Genre { get; set; } = string.Empty;
}