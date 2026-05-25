using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Scoring;

public interface IScoringCalculator
{
    double CalculateScore(UserCategoryProjection projection);
}
