using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Rules.Composite;

/// <summary>
/// Węzeł NOT — odwraca wynik dziecka (fuzzy NOT = 1 - value).
/// Analogia do NotRule/NotConstraint z Products.Domain.
/// </summary>
public class NotNode : IRuleNode
{
    private readonly IRuleNode _child;
    private readonly IScoreAlgebra _algebra;

    public NotNode(IScoreAlgebra algebra, IRuleNode child)
    {
        _algebra = algebra;
        _child = child;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        return _algebra.Not(_child.Evaluate(projection));
    }
}
