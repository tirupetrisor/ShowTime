namespace ShowTime.BusinessLogic.Dtos;

public class BookingGetDto
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public int UserId { get; set; }
    public int TicketId { get; set; }
    public string TicketType { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public string FestivalName { get; set; } = string.Empty;
    public string FestivalLocation { get; set; } = string.Empty;
    public DateTime FestivalStartDate { get; set; }
    public DateTime FestivalEndDate { get; set; }
    public string UserEmail { get; set; } = string.Empty;
} 