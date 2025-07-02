using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class ArtistService : IArtistService
{
    private readonly IRepository<Artist> _artistRepository;

    public ArtistService(IRepository<Artist> artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<ArtistGetDto?> GetArtistByIdAsync(int id)
    {
        try
        {
            var artist = await _artistRepository.GetByIdAsync(id);

            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }

            return new ArtistGetDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Image = artist.Image,
                Genre = artist.Genre
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the artist with ID {id}.", ex);
        }
    }

    public async Task<IList<ArtistGetDto>> GetAllArtistsAsync()
    {
        try
        {
            var artists = await _artistRepository.GetAllAsync();

            return artists.Select(artist => new ArtistGetDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Image = artist.Image,
                Genre = artist.Genre
            }).ToList();
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not implemented here)
            throw new Exception("An error occurred while retrieving all artists.", ex);
        }
    }

    public async Task AddArtistAsync(ArtistCreateDto artistCreateDto)
    {
        try
        {
            var artist = new Artist
            {
                Name = artistCreateDto.Name,
                Image = artistCreateDto.Image,
                Genre = artistCreateDto.Genre
            };

            await _artistRepository.AddAsync(artist);
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not implemented here)
            throw new Exception("An error occurred while adding the artist.", ex);
        }
    }

    public async Task UpdateArtistAsync(int id, ArtistUpdateDto artistUpdateDto)
    {
        try
        {
            var artist = await _artistRepository.GetByIdAsync(id);

            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }

            artist.Name = artistUpdateDto.Name;
            artist.Image = artistUpdateDto.Image;
            artist.Genre = artistUpdateDto.Genre;

            await _artistRepository.UpdateAsync(artist);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the artist with ID {id}.", ex);
        }
    }

    public async Task DeleteArtistAsync(int id)
    {
        await _artistRepository.DeleteAsync(id);
    }
}