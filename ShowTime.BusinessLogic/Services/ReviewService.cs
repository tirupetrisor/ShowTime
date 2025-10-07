using Microsoft.EntityFrameworkCore;
using ShowTime.BusinessLogic.Abstractions;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Festival> _festivalRepository;
    private readonly IRepository<User> _userRepository;
    private readonly ShowTimeDbContext _context;
    private readonly ISentimentService _sentiment;

    public ReviewService(
        IRepository<Review> reviewRepository,
        IRepository<Festival> festivalRepository,
        IRepository<User> userRepository,
        ShowTimeDbContext context,
        ISentimentService sentiment)
    {
        _reviewRepository = reviewRepository;
        _festivalRepository = festivalRepository;
        _userRepository = userRepository;
        _context = context;
        _sentiment = sentiment;
    }

    public async Task<IList<ReviewGetDto>> GetReviewsForFestivalAsync(int festivalId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.FestivalId == festivalId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(r => new ReviewGetDto
        {
            Id = r.Id,
            FestivalId = r.FestivalId,
            UserId = r.UserId,
            UserEmail = r.User.Email,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public async Task<double> GetAverageRatingForFestivalAsync(int festivalId)
    {
        var ratings = await _context.Reviews
            .Where(r => r.FestivalId == festivalId)
            .Select(r => (int?)r.Rating)
            .ToListAsync();

        if (!ratings.Any())
        {
            return 0.0d;
        }
        
        return ratings.Average(r => r ?? 0);
    }

    public async Task<ReviewGetDto> AddReviewAsync(int userId, ReviewCreateDto createDto)
    {
        var festival = await _festivalRepository.GetByIdAsync(createDto.FestivalId);
        if (festival == null)
        {
            throw new KeyNotFoundException($"Festival with ID {createDto.FestivalId} not found.");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        // ML.NET driven (with rating hint)
        var (label, score) = _sentiment.Analyze(createDto.Comment, createDto.Rating);

        var review = new Review
        {
            FestivalId = createDto.FestivalId,
            UserId = userId,
            Rating = createDto.Rating,
            Comment = createDto.Comment,
            SentimentLabel = label,
            SentimentScore = score,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);

        return new ReviewGetDto
        {
            Id = review.Id,
            FestivalId = review.FestivalId,
            UserId = review.UserId,
            UserEmail = user.Email,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task<(int Positive, int Neutral, int Negative, int Total)> GetSentimentSummaryAsync()
    {
        var all = await _context.Reviews.ToListAsync();

        // Backfill missing sentiments using ML.NET + rating hint
        bool changed = false;
        foreach (var r in all)
        {
            if (string.IsNullOrWhiteSpace(r.SentimentLabel))
            {
                var result = _sentiment.Analyze(r.Comment, r.Rating);
                r.SentimentLabel = result.Label;
                r.SentimentScore = result.Score;
                changed = true;
            }
        }
        if (changed)
        {
            await _context.SaveChangesAsync();
        }

        var pos = all.Count(r => r.SentimentLabel == "positive");
        var neu = all.Count(r => r.SentimentLabel == "neutral");
        var neg = all.Count(r => r.SentimentLabel == "negative");
        return (pos, neu, neg, all.Count);
    }

    public async Task<(int Positive, int Neutral, int Negative, int Total)> GetSentimentSummaryForFestivalAsync(int festivalId)
    {
        var all = await _context.Reviews
            .Where(r => r.FestivalId == festivalId)
            .ToListAsync();

        bool changed = false;
        foreach (var r in all)
        {
            if (string.IsNullOrWhiteSpace(r.SentimentLabel))
            {
                var result = _sentiment.Analyze(r.Comment, r.Rating);
                r.SentimentLabel = result.Label;
                r.SentimentScore = result.Score;
                changed = true;
            }
        }
        if (changed)
        {
            await _context.SaveChangesAsync();
        }

        var pos = all.Count(r => r.SentimentLabel == "positive");
        var neu = all.Count(r => r.SentimentLabel == "neutral");
        var neg = all.Count(r => r.SentimentLabel == "negative");
        return (pos, neu, neg, all.Count);
    }

    

    public async Task DeleteReviewAsync(int reviewId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new KeyNotFoundException($"Review with ID {reviewId} not found.");
        }
        await _reviewRepository.DeleteAsync(review);
    }
}


