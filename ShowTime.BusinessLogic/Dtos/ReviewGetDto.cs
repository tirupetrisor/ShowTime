namespace ShowTime.BusinessLogic.Dtos;

public class ReviewGetDto
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public int UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}


