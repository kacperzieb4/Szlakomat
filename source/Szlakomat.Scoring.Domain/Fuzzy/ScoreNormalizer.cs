using System;

namespace Szlakomat.Scoring.Domain.Fuzzy;

public class ScoreNormalizer : IScoreNormalizer
{
    private readonly double _maxScore;

    public ScoreNormalizer(double maxScore = 100.0)
    {
        _maxScore = maxScore;
    }

    public double Normalize(double rawScore)
    {
        if (rawScore <= 0) return 0.0;
        return Math.Min(rawScore / _maxScore, 1.0);
    }
}
