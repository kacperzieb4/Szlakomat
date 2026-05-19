using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Rules.Composite;

/// <summary>
/// Węzeł ważony — oblicza średnią ważoną wyników dzieci.
/// Każde dziecko ma swoją wagę, wynik = suma(waga * score) / suma(wag).
/// To rozszerzenie Composite Pattern specyficzne dla scoringu.
/// </summary>
public class WeightedNode : IRuleNode
{
    private readonly List<(IRuleNode Node, double Weight)> _weightedChildren;

    public WeightedNode(List<(IRuleNode Node, double Weight)> weightedChildren)
    {
        _weightedChildren = weightedChildren;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        if (_weightedChildren.Count == 0) return 0.0;

        double totalWeight = _weightedChildren.Sum(c => c.Weight);
        if (totalWeight <= 0) return 0.0;

        double weightedSum = _weightedChildren
            .Sum(c => c.Node.Evaluate(projection) * c.Weight);

        return weightedSum / totalWeight;
    }
}
