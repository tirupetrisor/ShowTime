using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class LineupService : ILineupService
{
    private readonly ILineupRepository _lineupRepository;

    public LineupService(ILineupRepository lineupRepository)
    {
        _lineupRepository = lineupRepository;
    }

    public async Task<LineupGetDto?> GetLineupByIdAsync(int festivalId, int artistId)
    {
        try
        {
            var lineup = await _lineupRepository.GetByIdAsync(festivalId, artistId);

            if (lineup == null)
            {
                throw new KeyNotFoundException($"Lineup with FestivalId {festivalId} and ArtistId {artistId} not found.");
            }

            return new LineupGetDto
            {
                FestivalId = lineup.FestivalId,
                ArtistId = lineup.ArtistId,
                Stage = lineup.Stage,
                StartTime = lineup.StartTime,
                FestivalName = lineup.Festival?.Name ?? string.Empty,
                ArtistName = lineup.Artist?.Name ?? string.Empty,
                FestivalLocation = lineup.Festival?.Location ?? string.Empty,
                ArtistGenre = lineup.Artist?.Genre ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the lineup with FestivalId {festivalId} and ArtistId {artistId}.", ex);
        }
    }

    public async Task<IList<LineupGetDto>> GetAllLineupsAsync()
    {
        try
        {
            var lineups = await _lineupRepository.GetAllAsync();

            return lineups.Select(lineup => new LineupGetDto
            {
                FestivalId = lineup.FestivalId,
                ArtistId = lineup.ArtistId,
                Stage = lineup.Stage,
                StartTime = lineup.StartTime,
                FestivalName = lineup.Festival?.Name ?? string.Empty,
                ArtistName = lineup.Artist?.Name ?? string.Empty,
                FestivalLocation = lineup.Festival?.Location ?? string.Empty,
                ArtistGenre = lineup.Artist?.Genre ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all lineups.", ex);
        }
    }

    public async Task<IList<LineupGetDto>> GetLineupsByFestivalIdAsync(int festivalId)
    {
        try
        {
            var lineups = await _lineupRepository.GetByFestivalIdAsync(festivalId);

            return lineups.Select(lineup => new LineupGetDto
            {
                FestivalId = lineup.FestivalId,
                ArtistId = lineup.ArtistId,
                Stage = lineup.Stage,
                StartTime = lineup.StartTime,
                FestivalName = lineup.Festival?.Name ?? string.Empty,
                ArtistName = lineup.Artist?.Name ?? string.Empty,
                FestivalLocation = lineup.Festival?.Location ?? string.Empty,
                ArtistGenre = lineup.Artist?.Genre ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving lineups for festival {festivalId}.", ex);
        }
    }

    public async Task<IList<LineupGetDto>> GetLineupsByArtistIdAsync(int artistId)
    {
        try
        {
            var lineups = await _lineupRepository.GetByArtistIdAsync(artistId);

            return lineups.Select(lineup => new LineupGetDto
            {
                FestivalId = lineup.FestivalId,
                ArtistId = lineup.ArtistId,
                Stage = lineup.Stage,
                StartTime = lineup.StartTime,
                FestivalName = lineup.Festival?.Name ?? string.Empty,
                ArtistName = lineup.Artist?.Name ?? string.Empty,
                FestivalLocation = lineup.Festival?.Location ?? string.Empty,
                ArtistGenre = lineup.Artist?.Genre ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving lineups for artist {artistId}.", ex);
        }
    }

    public async Task AddLineupAsync(LineupCreateDto lineupCreateDto)
    {
        try
        {
            var lineup = new Lineup
            {
                FestivalId = lineupCreateDto.FestivalId,
                ArtistId = lineupCreateDto.ArtistId,
                Stage = lineupCreateDto.Stage,
                StartTime = lineupCreateDto.StartTime
            };

            await _lineupRepository.AddAsync(lineup);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the lineup.", ex);
        }
    }

    public async Task UpdateLineupAsync(LineupUpdateDto lineupUpdateDto)
    {
        try
        {
            var lineup = await _lineupRepository.GetByIdAsync(lineupUpdateDto.FestivalId, lineupUpdateDto.ArtistId);

            if (lineup == null)
            {
                throw new KeyNotFoundException($"Lineup with FestivalId {lineupUpdateDto.FestivalId} and ArtistId {lineupUpdateDto.ArtistId} not found.");
            }

            lineup.Stage = lineupUpdateDto.Stage;
            lineup.StartTime = lineupUpdateDto.StartTime;

            await _lineupRepository.UpdateAsync(lineup);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the lineup.", ex);
        }
    }

    public async Task DeleteLineupAsync(int festivalId, int artistId)
    {
        try
        {
            await _lineupRepository.DeleteAsync(festivalId, artistId);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the lineup with FestivalId {festivalId} and ArtistId {artistId}.", ex);
        }
    }
} 