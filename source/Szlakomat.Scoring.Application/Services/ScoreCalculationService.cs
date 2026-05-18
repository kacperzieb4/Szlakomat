using Szlakomat.Scoring.Domain.Explanation;
using Szlakomat.Scoring.Domain.Fuzzy;

namespace Szlakomat.Scoring.Application.Services;

public class ScoreCalculationService
{
    private readonly IScoreAlgebra _algebra;

    public ScoreCalculationService(IScoreAlgebra algebra)
    {
        _algebra = algebra;
    }

    public ScoreResult CalculateFinalScore()
    {
        // Tutaj docelowo zintegrujemy Projekcje (Osoba 2) i Drzewo AST (Osoba 3).
        // Na ten moment zwracamy mocka.

        var result = new ScoreResult 
        { 
            Score = 0.5 
        };

        result.Explanations.Add(new ScoreExplanation { Reason = "Zalążek kalkulacji scoringu." });

        return result;
    }
}
