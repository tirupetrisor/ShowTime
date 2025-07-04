using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.DataAccess.Repositories.Implementations;

public class LineupRepository : ILineupRepository
{
    private readonly ShowTimeDbContext _context;

    public LineupRepository(ShowTimeDbContext context)
    {
        _context = context;
    }

    public async Task<Lineup?> GetByIdAsync(int festivalId, int artistId)
    {
        try
        {
            return await _context.Lineups
                .Include(l => l.Festival)
                .Include(l => l.Artist)
                .FirstOrDefaultAsync(l => l.FestivalId == festivalId && l.ArtistId == artistId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving lineup with FestivalId {festivalId} and ArtistId {artistId}", ex);
        }
    }

    public async Task<IEnumerable<Lineup>> GetAllAsync()
    {
        try
        {
            return await _context.Lineups
                .Include(l => l.Festival)
                .Include(l => l.Artist)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error retrieving all lineups", ex);
        }
    }

    public async Task<IEnumerable<Lineup>> GetByFestivalIdAsync(int festivalId)
    {
        try
        {
            return await _context.Lineups
                .Include(l => l.Festival)
                .Include(l => l.Artist)
                .Where(l => l.FestivalId == festivalId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving lineups for festival {festivalId}", ex);
        }
    }

    public async Task<IEnumerable<Lineup>> GetByArtistIdAsync(int artistId)
    {
        try
        {
            return await _context.Lineups
                .Include(l => l.Festival)
                .Include(l => l.Artist)
                .Where(l => l.ArtistId == artistId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving lineups for artist {artistId}", ex);
        }
    }

    public async Task AddAsync(Lineup lineup)
    {
        if (lineup == null)
            throw new ArgumentNullException(nameof(lineup));

        try
        {
            await _context.Lineups.AddAsync(lineup);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error adding lineup", ex);
        }
    }

    public async Task UpdateAsync(Lineup lineup)
    {
        if (lineup == null)
            throw new ArgumentNullException(nameof(lineup));

        try
        {
            _context.Entry(lineup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error updating lineup", ex);
        }
    }

    public async Task DeleteAsync(int festivalId, int artistId)
    {
        try
        {
            var lineup = await _context.Lineups
                .FirstOrDefaultAsync(l => l.FestivalId == festivalId && l.ArtistId == artistId);

            if (lineup == null)
                throw new KeyNotFoundException($"Lineup with FestivalId {festivalId} and ArtistId {artistId} not found.");

            _context.Lineups.Remove(lineup);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting lineup with FestivalId {festivalId} and ArtistId {artistId}", ex);
        }
    }
} 