using System;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Fuzzy.Nodes;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Application.Services;

public class ScoreCalculationService : IScoreCalculationService
{
    private readonly IScoreAlgebra _algebra;
    private readonly IScoreNormalizer _normalizer;
    private readonly IProjectionRepository _projections;
    private readonly IRuleNode _ruleTree;

    public ScoreCalculationService(
        IScoreAlgebra algebra, 
        IScoreNormalizer normalizer,
        IProjectionRepository projections,
        IRuleNode ruleTree)
    {
        _algebra = algebra;
        _normalizer = normalizer;
        _projections = projections;
        _ruleTree = ruleTree;
    }

    public async Task<ScoreResult> CalculateFinalScoreAsync(Guid userId, string category)
    {
        // Pobranie danych profilu aktywności użytkownika
        var projection = await _projections.GetAsync(userId, category);

        // Ewaluacja drzewa reguł AST
        double rawScore = _ruleTree.Evaluate(projection);

        // Normalizacja wyniku końcowego do przedziału [0.0, 1.0]
        double finalNormalizedScore = _normalizer.Normalize(rawScore);

        var result = new ScoreResult 
        { 
            Score = finalNormalizedScore 
        };

        // Dynamiczne budowanie wyjaśnień składowych oceny
        result.Explanations.Add(new ScoreExplanation 
        { 
            Reason = $"Poziom aktywności kliknięć ({projection.Clicks30Days}/10 w 30 dni).",
            Contribution = new FuzzyClicksNode().Evaluate(projection)
        });

        result.Explanations.Add(new ScoreExplanation 
        { 
            Reason = $"Zaangażowanie finansowe ({projection.Purchases90Days}/5 zakupów w 90 dni).",
            Contribution = new FuzzyPurchasesNode().Evaluate(projection)
        });

        result.Explanations.Add(new ScoreExplanation 
        { 
            Reason = $"Średnia ocena satysfakcji użytkownika ({projection.AverageRating:F1}/5.0).",
            Contribution = new FuzzyRatingNode().Evaluate(projection)
        });

        result.Explanations.Add(new ScoreExplanation 
        { 
            Reason = $"Wskaźnik braku znudzenia ({projection.Skips30Days}/10 pominięć w 30 dni).",
            Contribution = new FuzzySkipsNode().Evaluate(projection)
        });

        return result;
    }
}
