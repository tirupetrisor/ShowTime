using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.Services;

public class CartService
{
    public class CartItem
    {
        public int TicketId { get; set; }
        public int FestivalId { get; set; }
        public string FestivalName { get; set; } = string.Empty;
        public string TicketType { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    private readonly ITicketService _ticketService;
    private readonly IBookingService _bookingService;
    private readonly List<CartItem> _items = new();

    public event Action? OnChange;

    public CartService(ITicketService ticketService, IBookingService bookingService)
    {
        _ticketService = ticketService;
        _bookingService = bookingService;
    }

    public IReadOnlyList<CartItem> GetItems() => _items;

    public int GetTotalCount() => _items.Sum(i => i.Quantity);

    public decimal GetTotalPrice() => _items.Sum(i => i.UnitPrice * i.Quantity);

    public async Task<int> GetAvailableForTicketAsync(int ticketId)
    {
        try
        {
            return await _bookingService.GetAvailableTicketsForTypeAsync(ticketId);
        }
        catch
        {
            // If availability cannot be fetched (ticket deleted or transient error),
            // fail gracefully and report 0 available to the UI.
            return 0;
        }
    }

    public async Task AddAsync(int ticketId, int quantity)
    {
        if (quantity < 1) quantity = 1;

        var ticket = await _ticketService.GetTicketByIdAsync(ticketId) ?? throw new InvalidOperationException("Ticket not found");

        var available = await _bookingService.GetAvailableTicketsForTypeAsync(ticketId);
        var existing = _items.FirstOrDefault(i => i.TicketId == ticketId);
        var desiredTotal = (existing?.Quantity ?? 0) + quantity;

        if (desiredTotal > available)
        {
            throw new InvalidOperationException("Not enough tickets available for this type.");
        }

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                TicketId = ticket.Id,
                FestivalId = ticket.FestivalId,
                FestivalName = ticket.FestivalName,
                TicketType = ticket.Type,
                UnitPrice = ticket.Price,
                Quantity = quantity
            });
        }

        OnChange?.Invoke();
    }

    public void UpdateQuantity(int ticketId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.TicketId == ticketId) ?? throw new InvalidOperationException("Item not found in cart");
        if (quantity < 1) quantity = 1;
        item.Quantity = quantity;
        OnChange?.Invoke();
    }

    public void Remove(int ticketId)
    {
        var item = _items.FirstOrDefault(i => i.TicketId == ticketId);
        if (item != null)
        {
            _items.Remove(item);
            OnChange?.Invoke();
        }
    }

    public void Clear()
    {
        _items.Clear();
        OnChange?.Invoke();
    }

    public async Task CheckoutAsync(int userId)
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        var groups = _items
            .GroupBy(i => new { i.FestivalId, i.TicketId })
            .Select(g => new { g.Key.FestivalId, g.Key.TicketId, Quantity = g.Sum(x => x.Quantity) })
            .ToList();

        foreach (var g in groups)
        {
            var bookingDto = new BookingCreateDto
            {
                FestivalId = g.FestivalId,
                TicketId = g.TicketId,
                UserId = userId,
                Quantity = g.Quantity
            };

            await _bookingService.CreateBookingAsync(bookingDto);
        }

        Clear();
    }
}


