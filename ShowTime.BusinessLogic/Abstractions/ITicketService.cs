using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface ITicketService
{
    Task<TicketGetDto?> GetTicketByIdAsync(int id);
    Task<IList<TicketGetDto>> GetAllTicketsAsync();
    Task<IList<TicketGetDto>> GetTicketsByFestivalIdAsync(int festivalId);
    Task<bool> CreateTicketAsync(TicketCreateDto ticketCreateDto);
    Task<bool> UpdateTicketAsync(int id, TicketUpdateDto ticketUpdateDto);
    Task<bool> DeleteTicketAsync(int id); // Verifică că nu există bookings
} 