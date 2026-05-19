using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Rules.Composite;

/// <summary>
/// Węzeł OR — zwraca MAXIMUM z wyników dzieci (fuzzy OR = max).
/// Odpowiada logice: "wystarczy, że jeden warunek jest spełniony".
/// Analogia do OrRule/OrConstraint z Products.Domain.
/// </summary>
public class OrNode : IRuleNode
{
    private readonly List<IRuleNode> _children;
    private readonly IScoreAlgebra _algebra;

    public OrNode(IScoreAlgebra algebra, List<IRuleNode> children)
    {
        _algebra = algebra;
        _children = children;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        var scores = _children.Select(c => c.Evaluate(projection));
        return _algebra.Or(scores);
    }
}
