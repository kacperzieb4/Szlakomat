using System;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Domain.Fuzzy.Nodes;

public class FuzzySkipsNode : IRuleNode
{
    private readonly int _maxSkips;

    public FuzzySkipsNode(int maxSkips = 10)
    {
        _maxSkips = maxSkips;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        return 1.0 - Math.Min(projection.Skips30Days / (double)_maxSkips, 1.0);
    }
}
