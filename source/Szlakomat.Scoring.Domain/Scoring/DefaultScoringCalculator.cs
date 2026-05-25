using System;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Scoring;

public class DefaultScoringCalculator : IScoringCalculator
{
    public double CalculateScore(UserCategoryProjection projection)
    {
        if (projection == null)
        {
            return 0.0;
        }

        // clicks increase score slightly, purchases strongly, skips decrease
        double rawScore = (projection.Clicks30Days * 0.05) 
                          + (projection.Purchases90Days * 0.20) 
                          - (projection.Skips30Days * 0.10);

        // normalize result to range [0.0, 1.0]
        return Math.Clamp(rawScore, 0.0, 1.0);
    }
}
