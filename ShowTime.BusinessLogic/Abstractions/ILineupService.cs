using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface ILineupService
{
    Task<LineupGetDto?> GetLineupByIdAsync(int festivalId, int artistId);
    Task<IList<LineupGetDto>> GetAllLineupsAsync();
    Task<IList<LineupGetDto>> GetLineupsByFestivalIdAsync(int festivalId);
    Task<IList<LineupGetDto>> GetLineupsByArtistIdAsync(int artistId);
    Task AddLineupAsync(LineupCreateDto lineupCreateDto);
    Task UpdateLineupAsync(LineupUpdateDto lineupUpdateDto);
    Task DeleteLineupAsync(int festivalId, int artistId);
} 