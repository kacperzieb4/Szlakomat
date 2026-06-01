using System;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Domain.Fuzzy.Nodes;

public class FuzzyPurchasesNode : IRuleNode
{
    private readonly int _maxPurchases;

    public FuzzyPurchasesNode(int maxPurchases = 5)
    {
        _maxPurchases = maxPurchases;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        return Math.Min(projection.Purchases90Days / (double)_maxPurchases, 1.0);
    }
}
