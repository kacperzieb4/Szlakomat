using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Rules.Composite;

namespace Szlakomat.Scoring.Domain.Rules;

/// <summary>
/// Fluent builder do składania drzewa reguł AST.
/// Pozwala w czytelny sposób komponować composite nodes.
/// Inspiracja: ProductBuilder i dwufazowy design z Products.Domain.
/// </summary>
public class RuleTreeBuilder
{
    private readonly IScoreAlgebra _algebra;

    public RuleTreeBuilder(IScoreAlgebra algebra)
    {
        _algebra = algebra;
    }

    public AndNode And(params IRuleNode[] children)
    {
        return new AndNode(_algebra, children.ToList());
    }

    public OrNode Or(params IRuleNode[] children)
    {
        return new OrNode(_algebra, children.ToList());
    }

    public NotNode Not(IRuleNode child)
    {
        return new NotNode(_algebra, child);
    }

    public IfThenNode IfThen(
        IRuleNode condition,
        IRuleNode thenNode,
        double threshold = 0.5,
        double elseScore = 0.0)
    {
        return new IfThenNode(condition, thenNode, threshold, elseScore);
    }

    public WeightedNode Weighted(params (IRuleNode Node, double Weight)[] children)
    {
        return new WeightedNode(children.ToList());
    }
}
