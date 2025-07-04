namespace ShowTime.BusinessLogic.Dtos;

public class LineupUpdateDto
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
} 