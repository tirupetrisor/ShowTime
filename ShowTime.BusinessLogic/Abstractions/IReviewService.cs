using ShowTime.BusinessLogic.Dtos;

namespace ShowTime.BusinessLogic.Abstractions;

public interface IReviewService
{
    Task<IList<ReviewGetDto>> GetReviewsForFestivalAsync(int festivalId);
    Task<double> GetAverageRatingForFestivalAsync(int festivalId);
    Task<ReviewGetDto> AddReviewAsync(int userId, ReviewCreateDto createDto);
    Task DeleteReviewAsync(int reviewId);
    Task<(int Positive, int Neutral, int Negative, int Total)> GetSentimentSummaryAsync();
    Task<(int Positive, int Neutral, int Negative, int Total)> GetSentimentSummaryForFestivalAsync(int festivalId);
}


