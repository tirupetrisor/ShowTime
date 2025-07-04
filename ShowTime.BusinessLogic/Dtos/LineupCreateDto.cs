namespace ShowTime.BusinessLogic.Dtos;

public class LineupCreateDto
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; } = DateTime.Today.AddHours(18); // Default to 6 PM today
} 