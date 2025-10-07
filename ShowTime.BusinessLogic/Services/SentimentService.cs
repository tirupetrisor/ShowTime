using Microsoft.ML;
using Microsoft.ML.Data;
using ShowTime.BusinessLogic.Abstractions;

namespace ShowTime.BusinessLogic.Services;

public class SentimentService : ISentimentService
{
    private readonly object _lock = new();
    private PredictionEngine<SentimentData, SentimentPrediction>? _engine;

    public SentimentService()
    {
        Initialize();
    }

    public (string Label, double Score) Analyze(string text, int ratingHint = 0)
    {
        if (ratingHint >= 4) return ("positive", 0.95);
        if (ratingHint <= 2 && ratingHint > 0) return ("negative", 0.95);

        if (string.IsNullOrWhiteSpace(text)) return ("neutral", 0.5);

        EnsureEngine();
        var prediction = _engine!.Predict(new SentimentData { Text = text });
        var label = prediction.Prediction ? "positive" : "negative";
        var score = prediction.Probability;
        if (score < 0.6) return ("neutral", 0.55);
        return (label, score);
    }

    private void Initialize()
    {
        try
        {
            EnsureEngine();
        }
        catch
        {
            _engine = null;
        }
    }

    private void EnsureEngine()
    {
        if (_engine != null) return;
        lock (_lock)
        {
            if (_engine != null) return;
            var ml = new MLContext();
            var pipeline = ml.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            // tiny self-trained model on-the-fly fallback; in real usage load a pre-trained model file
            var empty = ml.Data.LoadFromEnumerable(new List<SentimentData>
            {
                new() { Text = "great" },
                new() { Text = "amazing" },
                new() { Text = "bad" },
                new() { Text = "terrible" }
            });
            var labeled = ml.Transforms.CustomMapping<SentimentData, SentimentLabel>((s, l) =>
            {
                var t = (s.Text ?? string.Empty).ToLowerInvariant();
                l.Label = t.Contains("great") || t.Contains("amazing");
            }, contractName: "SentimentLabelMapping").Fit(empty).Transform(empty);

            var model = pipeline.Fit(labeled);
            _engine = ml.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        }
    }

    private class SentimentData
    {
        public string Text { get; set; } = string.Empty;
    }

    private class SentimentLabel
    {
        public bool Label { get; set; }
    }

    private class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}


