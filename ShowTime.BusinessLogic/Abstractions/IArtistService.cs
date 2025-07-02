using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IArtistService
{
    Task<ArtistGetDto?> GetArtistByIdAsync(int id);
    Task<IList<ArtistGetDto>> GetAllArtistsAsync();
    Task AddArtistAsync(ArtistCreateDto artistCreateDto);
    Task UpdateArtistAsync(int id, ArtistUpdateDto artistUpdateDto);
    Task DeleteArtistAsync(int id);
}