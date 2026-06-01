using System;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Domain.Fuzzy.Nodes;

public class FuzzyClicksNode : IRuleNode
{
    private readonly int _maxClicks;

    public FuzzyClicksNode(int maxClicks = 10)
    {
        _maxClicks = maxClicks;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        return Math.Min(projection.Clicks30Days / (double)_maxClicks, 1.0);
    }
}
