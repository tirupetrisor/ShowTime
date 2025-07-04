using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IFestivalService
{
    Task<FestivalGetDto?> GetFestivalByIdAsync(int id);
    Task<IList<FestivalGetDto>> GetAllFestivalsAsync();
    Task AddFestivalAsync(FestivalCreateDto festivalCreateDto);
    Task UpdateFestivalAsync(FestivalUpdateDto festivalUpdateDto);
    Task DeleteFestivalAsync(int id);
} 