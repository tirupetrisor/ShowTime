using System.ComponentModel.DataAnnotations;

namespace ShowTime.BusinessLogic.Dtos;

public class FestivalCreateDto : IValidatableObject
{
    [Required(ErrorMessage = "Festival name is required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Festival name must be between 2 and 150 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Location is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 200 characters")]
    public string Location { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; } = DateTime.Today;
    
    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
    
    [Required(ErrorMessage = "Splash art URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string SplashArt { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 1000000, ErrorMessage = "Capacity must be between 1 and 1,000,000")]
    public int Capacity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate <= StartDate)
        {
            yield return new ValidationResult(
                "End date must be after start date",
                new[] { nameof(EndDate) });
        }

        if (StartDate < DateTime.Today)
        {
            yield return new ValidationResult(
                "Start date cannot be in the past",
                new[] { nameof(StartDate) });
        }
    }
} 