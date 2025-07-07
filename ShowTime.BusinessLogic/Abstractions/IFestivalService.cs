using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IFestivalService
{
    // Festival methods
    Task<FestivalGetDto?> GetFestivalByIdAsync(int id);
    Task<IList<FestivalGetDto>> GetAllFestivalsAsync();
    Task AddFestivalAsync(FestivalCreateDto festivalCreateDto);
    Task UpdateFestivalAsync(FestivalUpdateDto festivalUpdateDto);
    Task DeleteFestivalAsync(int id);
    
    // Lineup methods integrated into Festival service
    Task<LineupGetDto?> GetLineupByIdAsync(int festivalId, int artistId);
    Task<IList<LineupGetDto>> GetAllLineupsAsync();
    Task<IList<LineupGetDto>> GetLineupsByFestivalIdAsync(int festivalId);
    Task AddLineupAsync(LineupCreateDto lineupCreateDto);
    Task UpdateLineupAsync(LineupUpdateDto lineupUpdateDto);
    Task DeleteLineupAsync(int festivalId, int artistId);
} 