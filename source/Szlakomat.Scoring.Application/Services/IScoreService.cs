using Szlakomat.Scoring.Domain.Explanation;

namespace Szlakomat.Scoring.Application.Services;

public interface IScoreService
{
    Task<ScoreResult> Calculate(
        Guid userId,
        string category);
}