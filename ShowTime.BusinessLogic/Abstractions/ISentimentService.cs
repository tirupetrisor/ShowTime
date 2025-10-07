namespace ShowTime.BusinessLogic.Abstractions;

public interface ISentimentService
{
    (string Label, double Score) Analyze(string text, int ratingHint = 0);
}


