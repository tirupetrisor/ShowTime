using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class FestivalService : IFestivalService
{
    private readonly IRepository<Festival> _festivalRepository;
    private readonly ILineupRepository _lineupRepository;

    public FestivalService(IRepository<Festival> festivalRepository, ILineupRepository lineupRepository)
    {
        _festivalRepository = festivalRepository;
        _lineupRepository = lineupRepository;
    }

    public async Task<FestivalGetDto?> GetFestivalByIdAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);

            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            return new FestivalGetDto
            {
                Id = festival.Id,
                Name = festival.Name,
                Location = festival.Location,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                SplashArt = festival.SplashArt,
                Capacity = festival.Capacity
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the festival with ID {id}.", ex);
        }
    }

    public async Task<IList<FestivalGetDto>> GetAllFestivalsAsync()
    {
        try
        {
            var festivals = await _festivalRepository.GetAllAsync();

            return festivals.Select(festival => new FestivalGetDto
            {
                Id = festival.Id,
                Name = festival.Name,
                Location = festival.Location,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                SplashArt = festival.SplashArt,
                Capacity = festival.Capacity
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all festivals.", ex);
        }
    }

    public async Task AddFestivalAsync(FestivalCreateDto festivalCreateDto)
    {
        try
        {
            var festival = new Festival
            {
                Name = festivalCreateDto.Name,
                Location = festivalCreateDto.Location,
                StartDate = festivalCreateDto.StartDate,
                EndDate = festivalCreateDto.EndDate,
                SplashArt = festivalCreateDto.SplashArt,
                Capacity = festivalCreateDto.Capacity
            };

            await _festivalRepository.AddAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the festival.", ex);
        }
    }

    public async Task UpdateFestivalAsync(FestivalUpdateDto festivalUpdateDto)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(festivalUpdateDto.Id);

            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {festivalUpdateDto.Id} not found.");
            }

            festival.Name = festivalUpdateDto.Name;
            festival.Location = festivalUpdateDto.Location;
            festival.StartDate = festivalUpdateDto.StartDate;
            festival.EndDate = festivalUpdateDto.EndDate;
            festival.SplashArt = festivalUpdateDto.SplashArt;
            festival.Capacity = festivalUpdateDto.Capacity;

            await _festivalRepository.UpdateAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the festival.", ex);
        }
    }

    public async Task DeleteFestivalAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);

            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            await _festivalRepository.DeleteAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the festival with ID {id}.", ex);
        }
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