using System;

namespace ShowTime.DataAccess.Models;

public class Review
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string SentimentLabel { get; set; } = string.Empty; // positive/neutral/negative
    public double SentimentScore { get; set; } = 0d; // confidence 0..1

    public Festival Festival { get; set; } = null!;
    public User User { get; set; } = null!;
}


