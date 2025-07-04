using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class FestivalService : IFestivalService
{
    private readonly IRepository<Festival> _festivalRepository;

    public FestivalService(IRepository<Festival> festivalRepository)
    {
        _festivalRepository = festivalRepository;
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
} 