using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IBookingService
{
    Task<BookingGetDto?> GetBookingByIdAsync(int id);
    Task<IList<BookingGetDto>> GetAllBookingsAsync(); // Doar pentru admin
    Task<IList<BookingGetDto>> GetBookingsByUserIdAsync(int userId);
    Task<IList<BookingGetDto>> GetBookingsByFestivalIdAsync(int festivalId);
    Task<bool> CreateBookingAsync(BookingCreateDto bookingCreateDto);
    Task<bool> UpdateBookingAsync(int id, int newTicketId); // Schimbă tipul biletului
    Task<bool> CancelBookingAsync(int id);
    Task<bool> DeleteBookingAsync(int id); // Admin functionality to delete any booking
    Task<bool> IsUserBookedForFestivalAsync(int festivalId, int userId);
    Task<int> GetAvailableTicketsAsync(int festivalId);
    Task<Dictionary<string, int>> GetBookingStatsByFestivalAsync(int festivalId); // Admin only
} 