using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.Scoring;

namespace Szlakomat.Scoring.Application.Services;

public class ScoreService : IScoreService
{
    private readonly IScoringCalculator _calculator;
    private readonly IProjectionRepository _projectionRepository;

    public ScoreService(IScoringCalculator calculator, IProjectionRepository projectionRepository)
    {
        _calculator = calculator;
        _projectionRepository = projectionRepository;
    }

    public async Task<ScoreResult> Calculate(Guid userId, string category)
    {
        var projection = await _projectionRepository.GetAsync(userId, category);
        if (projection == null)
        {
            return new ScoreResult
            {
                Score = 0.0,
                Reasons = new List<string> { "No scoring history found for this category." }
            };
        }

        var score = _calculator.CalculateScore(projection);

        return new ScoreResult
        {
            Score = score,
            Reasons = new List<string>
            {
                $"Score computed from activity: {projection.Clicks30Days} clicks, {projection.Purchases90Days} purchases, {projection.Skips30Days} skips.",
                $"Average category rating: {projection.AverageRating.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}."
            }
        };
    }
}
