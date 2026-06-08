using System;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Application.Services;

public interface IScoreCalculationService
{
    Task<ScoreResult> CalculateFinalScoreAsync(Guid userId, TrailCategory category);
}
