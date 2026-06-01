using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Rules.Composite;

/// <summary>
/// Węzeł AND — zwraca MINIMUM z wyników dzieci (fuzzy AND = min).
/// Odpowiada logice: "wszystkie warunki muszą być spełnione" —
/// najsłabsze ogniwo determinuje wynik całości.
/// Analogia do AndRule/AndConstraint z Products.Domain.
/// </summary>
public class AndNode : IRuleNode
{
    private readonly List<IRuleNode> _children;
    private readonly IScoreAlgebra _algebra;

    public AndNode(IScoreAlgebra algebra, List<IRuleNode> children)
    {
        _algebra = algebra;
        _children = children;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        var scores = _children.Select(c => c.Evaluate(projection));
        return _algebra.And(scores);
    }
}
