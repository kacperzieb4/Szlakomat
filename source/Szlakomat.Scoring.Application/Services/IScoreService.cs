using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Application.Services;

public interface IScoreService
{
    Task<ScoreResult> Calculate(
        Guid userId,
        TrailCategory category);
}