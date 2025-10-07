namespace ShowTime.BusinessLogic.Dtos;

public class TicketGetDto
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FestivalName { get; set; } = string.Empty;
    public int Capacity { get; set; }
} 