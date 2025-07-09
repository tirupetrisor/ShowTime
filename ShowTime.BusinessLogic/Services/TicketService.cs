using Microsoft.EntityFrameworkCore;
using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class TicketService : ITicketService
{
    private readonly IRepository<Ticket> _ticketRepository;
    private readonly IRepository<Festival> _festivalRepository;
    private readonly IRepository<Booking> _bookingRepository;
    private readonly ShowTimeDbContext _context;

    public TicketService(
        IRepository<Ticket> ticketRepository,
        IRepository<Festival> festivalRepository,
        IRepository<Booking> bookingRepository,
        ShowTimeDbContext context)
    {
        _ticketRepository = ticketRepository;
        _festivalRepository = festivalRepository;
        _bookingRepository = bookingRepository;
        _context = context;
    }

    public async Task<TicketGetDto?> GetTicketByIdAsync(int id)
    {
        try
        {
            var ticket = await _context.Tickets
                .Include(t => t.Festival)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return null;
            }

            return new TicketGetDto
            {
                Id = ticket.Id,
                FestivalId = ticket.FestivalId,
                Type = ticket.Type,
                Price = ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = ticket.Festival.Name
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the ticket with ID {id}.", ex);
        }
    }

    public async Task<IList<TicketGetDto>> GetAllTicketsAsync()
    {
        try
        {
            var tickets = await _context.Tickets
                .Include(t => t.Festival)
                .ToListAsync();

            return tickets.Select(ticket => new TicketGetDto
            {
                Id = ticket.Id,
                FestivalId = ticket.FestivalId,
                Type = ticket.Type,
                Price = ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = ticket.Festival.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all tickets.", ex);
        }
    }

    public async Task<IList<TicketGetDto>> GetTicketsByFestivalIdAsync(int festivalId)
    {
        try
        {
            var tickets = await _context.Tickets
                .Include(t => t.Festival)
                .Where(t => t.FestivalId == festivalId)
                .ToListAsync();

            return tickets.Select(ticket => new TicketGetDto
            {
                Id = ticket.Id,
                FestivalId = ticket.FestivalId,
                Type = ticket.Type,
                Price = ticket.Price / 100m, // Convert from cents to decimal
                FestivalName = ticket.Festival.Name
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving tickets for festival {festivalId}.", ex);
        }
    }

    public async Task<bool> CreateTicketAsync(TicketCreateDto ticketCreateDto)
    {
        try
        {
            // Verifică dacă festivalul există
            var festival = await _festivalRepository.GetByIdAsync(ticketCreateDto.FestivalId);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {ticketCreateDto.FestivalId} not found.");
            }

            var ticket = new Ticket
            {
                FestivalId = ticketCreateDto.FestivalId,
                Type = ticketCreateDto.Type,
                Price = (int)(ticketCreateDto.Price * 100) // Convert to cents for storage
            };

            await _ticketRepository.AddAsync(ticket);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the ticket.", ex);
        }
    }

    public async Task<bool> UpdateTicketAsync(int id, TicketUpdateDto ticketUpdateDto)
    {
        try
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            ticket.Type = ticketUpdateDto.Type;
            ticket.Price = (int)(ticketUpdateDto.Price * 100); // Convert to cents for storage

            await _ticketRepository.UpdateAsync(ticket);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the ticket.", ex);
        }
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        try
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            // Verifică dacă există rezervări pentru acest bilet
            var bookings = await _context.Bookings
                .Where(b => b.TicketId == id)
                .ToListAsync();

            if (bookings.Any())
            {
                throw new InvalidOperationException("Cannot delete ticket with existing bookings.");
            }

            await _ticketRepository.DeleteAsync(ticket);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the ticket with ID {id}.", ex);
        }
    }
} 