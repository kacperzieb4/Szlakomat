using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.Fuzzy;

namespace Szlakomat.Scoring.Application.Services;

public class ScoreCalculationService
{
    private readonly IScoreAlgebra _algebra;
    private readonly IScoreNormalizer _normalizer;

    public ScoreCalculationService(IScoreAlgebra algebra, IScoreNormalizer normalizer)
    {
        _algebra = algebra;
        _normalizer = normalizer;
    }

    public ScoreResult CalculateFinalScore()
    {
        // Poniższa wartość zostanie w przyszłości zastąpiona wynikiem ewaluacji reguł AST i projekcji.
        double rawScore = 50.0;

        // Normalizacja wyniku: zabezpieczenie przed wartościami spoza przedziału 0.0 - 1.0.
        // Każdy wyliczony wynik bezwzględnie przechodzi przez warstwę normalizacji.
        double finalNormalizedScore = _normalizer.Normalize(rawScore);

        var result = new ScoreResult 
        { 
            Score = finalNormalizedScore 
        };

        result.Explanations.Add(new ScoreExplanation { Reason = "Przykładowe uzasadnienie wyniku bazowego." });

        return result;
    }
}
