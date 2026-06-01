using System.Collections.Generic;

namespace Szlakomat.Scoring.Domain.Explanation;

public class ScoreResult
{
    public double Score { get; set; }
    public List<ScoreExplanation> Explanations { get; set; } = new();
    public List<string> Reasons { get; set; } = new();
}