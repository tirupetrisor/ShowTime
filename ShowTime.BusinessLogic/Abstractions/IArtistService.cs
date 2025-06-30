using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IArtistService
{
    Task<ArtistGetDto?> GetArtistByIdAsync(int id);
    Task<IEnumerable<ArtistGetDto>> GetAllArtistsAsync();
    Task AddArtistAsync(Artist artist);
    Task UpdateArtistAsync(Artist artist);
    Task DeleteArtistAsync(int id);
}