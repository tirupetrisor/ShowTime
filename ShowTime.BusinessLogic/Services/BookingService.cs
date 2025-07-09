using Microsoft.EntityFrameworkCore;
using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class BookingService : IBookingService
{
    private readonly IRepository<Booking> _bookingRepository;
    private readonly IRepository<Festival> _festivalRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Ticket> _ticketRepository;
    private readonly ShowTimeDbContext _context;

    public BookingService(
        IRepository<Booking> bookingRepository,
        IRepository<Festival> festivalRepository,
        IRepository<User> userRepository,
        IRepository<Ticket> ticketRepository,
        ShowTimeDbContext context)
    {
        _bookingRepository = bookingRepository;
        _festivalRepository = festivalRepository;
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _context = context;
    }

    public async Task<BookingGetDto?> GetBookingByIdAsync(int id)
    {
        try
        {
            var booking = await _context.Bookings
                .Include(b => b.Festival)
                .Include(b => b.User)
                .Include(b => b.Ticket)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return null;
            }

            return new BookingGetDto
            {
                Id = booking.Id,
                FestivalId = booking.FestivalId,
                UserId = booking.UserId,
                TicketId = booking.TicketId,
                TicketType = booking.Ticket.Type,
                TicketPrice = booking.Ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = booking.Festival.Name,
                FestivalLocation = booking.Festival.Location,
                FestivalStartDate = booking.Festival.StartDate,
                FestivalEndDate = booking.Festival.EndDate,
                UserEmail = booking.User.Email
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the booking with ID {id}.", ex);
        }
    }

    public async Task<IList<BookingGetDto>> GetAllBookingsAsync()
    {
        try
        {
            var bookings = await _context.Bookings
                .Include(b => b.Festival)
                .Include(b => b.User)
                .Include(b => b.Ticket)
                .ToListAsync();

            return bookings.Select(booking => new BookingGetDto
            {
                Id = booking.Id,
                FestivalId = booking.FestivalId,
                UserId = booking.UserId,
                TicketId = booking.TicketId,
                TicketType = booking.Ticket.Type,
                TicketPrice = booking.Ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = booking.Festival.Name,
                FestivalLocation = booking.Festival.Location,
                FestivalStartDate = booking.Festival.StartDate,
                FestivalEndDate = booking.Festival.EndDate,
                UserEmail = booking.User.Email
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all bookings.", ex);
        }
    }

    public async Task<IList<BookingGetDto>> GetBookingsByUserIdAsync(int userId)
    {
        try
        {
            var bookings = await _context.Bookings
                .Include(b => b.Festival)
                .Include(b => b.User)
                .Include(b => b.Ticket)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return bookings.Select(booking => new BookingGetDto
            {
                Id = booking.Id,
                FestivalId = booking.FestivalId,
                UserId = booking.UserId,
                TicketId = booking.TicketId,
                TicketType = booking.Ticket.Type,
                TicketPrice = booking.Ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = booking.Festival.Name,
                FestivalLocation = booking.Festival.Location,
                FestivalStartDate = booking.Festival.StartDate,
                FestivalEndDate = booking.Festival.EndDate,
                UserEmail = booking.User.Email
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving bookings for user {userId}.", ex);
        }
    }

    public async Task<IList<BookingGetDto>> GetBookingsByFestivalIdAsync(int festivalId)
    {
        try
        {
            var bookings = await _context.Bookings
                .Include(b => b.Festival)
                .Include(b => b.User)
                .Include(b => b.Ticket)
                .Where(b => b.FestivalId == festivalId)
                .ToListAsync();

            return bookings.Select(booking => new BookingGetDto
            {
                Id = booking.Id,
                FestivalId = booking.FestivalId,
                UserId = booking.UserId,
                TicketId = booking.TicketId,
                TicketType = booking.Ticket.Type,
                TicketPrice = booking.Ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = booking.Festival.Name,
                FestivalLocation = booking.Festival.Location,
                FestivalStartDate = booking.Festival.StartDate,
                FestivalEndDate = booking.Festival.EndDate,
                UserEmail = booking.User.Email
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving bookings for festival {festivalId}.", ex);
        }
    }

    public async Task<bool> CreateBookingAsync(BookingCreateDto bookingCreateDto)
    {
        try
        {
            // Verifică dacă festivalul există
            var festival = await _festivalRepository.GetByIdAsync(bookingCreateDto.FestivalId);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {bookingCreateDto.FestivalId} not found.");
            }

            // Verifică dacă utilizatorul există
            var user = await _userRepository.GetByIdAsync(bookingCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {bookingCreateDto.UserId} not found.");
            }

            // Verifică dacă tipul de bilet există
            var ticket = await _ticketRepository.GetByIdAsync(bookingCreateDto.TicketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {bookingCreateDto.TicketId} not found.");
            }

            // Verifică dacă biletul aparține festivalului
            if (ticket.FestivalId != bookingCreateDto.FestivalId)
            {
                throw new InvalidOperationException("Ticket does not belong to the specified festival.");
            }

            // Verifică dacă utilizatorul are deja o rezervare pentru acest festival
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == bookingCreateDto.UserId && b.FestivalId == bookingCreateDto.FestivalId);

            if (existingBooking != null)
            {
                throw new InvalidOperationException("User already has a booking for this festival.");
            }

            // Verifică capacitatea festivalului
            var currentBookings = await _context.Bookings
                .Where(b => b.FestivalId == bookingCreateDto.FestivalId)
                .CountAsync();

            if (currentBookings >= festival.Capacity)
            {
                throw new InvalidOperationException("Festival is at full capacity.");
            }

            var booking = new Booking
            {
                FestivalId = bookingCreateDto.FestivalId,
                UserId = bookingCreateDto.UserId,
                TicketId = bookingCreateDto.TicketId
            };

            await _bookingRepository.AddAsync(booking);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the booking.", ex);
        }
    }

    public async Task<bool> UpdateBookingAsync(int id, int newTicketId)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }

            // Verifică dacă noul bilet există
            var newTicket = await _ticketRepository.GetByIdAsync(newTicketId);
            if (newTicket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {newTicketId} not found.");
            }

            // Verifică dacă noul bilet aparține aceluiași festival
            if (newTicket.FestivalId != booking.FestivalId)
            {
                throw new InvalidOperationException("New ticket does not belong to the same festival.");
            }

            booking.TicketId = newTicketId;
            await _bookingRepository.UpdateAsync(booking);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the booking.", ex);
        }
    }

    public async Task<bool> CancelBookingAsync(int id)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }

            await _bookingRepository.DeleteAsync(booking);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while canceling the booking with ID {id}.", ex);
        }
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }

            await _bookingRepository.DeleteAsync(booking);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the booking with ID {id}.", ex);
        }
    }

    public async Task<bool> IsUserBookedForFestivalAsync(int festivalId, int userId)
    {
        try
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.FestivalId == festivalId && b.UserId == userId);

            return booking != null;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while checking if user {userId} is booked for festival {festivalId}.", ex);
        }
    }

    public async Task<int> GetAvailableTicketsAsync(int festivalId)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(festivalId);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {festivalId} not found.");
            }

            var currentBookings = await _context.Bookings
                .Where(b => b.FestivalId == festivalId)
                .CountAsync();

            return Math.Max(0, festival.Capacity - currentBookings);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting available tickets for festival {festivalId}.", ex);
        }
    }

    public async Task<Dictionary<string, int>> GetBookingStatsByFestivalAsync(int festivalId)
    {
        try
        {
            var bookingStats = await _context.Bookings
                .Include(b => b.Ticket)
                .Where(b => b.FestivalId == festivalId)
                .GroupBy(b => b.Ticket.Type)
                .Select(g => new { TicketType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.TicketType, x => x.Count);

            return bookingStats;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while getting booking statistics for festival {festivalId}.", ex);
        }
    }
} 