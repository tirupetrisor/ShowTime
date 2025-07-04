namespace ShowTime.BusinessLogic.Dtos;

public class LineupGetDto
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    
    // Navigation properties for display
    public string FestivalName { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string FestivalLocation { get; set; } = string.Empty;
    public string ArtistGenre { get; set; } = string.Empty;
} 