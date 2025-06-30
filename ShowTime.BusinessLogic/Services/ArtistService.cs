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
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ArtistGetDto>> GetAllArtistsAsync()
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
            });
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not implemented here)
            throw new Exception("An error occurred while retrieving all artists.", ex);
        }
    }

    public async Task AddArtistAsync(Artist artist)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateArtistAsync(Artist artist)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteArtistAsync(int id)
    {
        throw new NotImplementedException();
    }
}