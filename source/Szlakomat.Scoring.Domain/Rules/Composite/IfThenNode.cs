using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Rules.Composite;

/// <summary>
/// Węzeł warunkowy IF-THEN — jeśli condition >= threshold,
/// zwraca wynik thenNode, w przeciwnym razie zwraca elseScore.
/// Analogia do ConditionalRule z Products.Domain.
/// </summary>
public class IfThenNode : IRuleNode
{
    private readonly IRuleNode _condition;
    private readonly IRuleNode _thenNode;
    private readonly double _threshold;
    private readonly double _elseScore;

    public IfThenNode(
        IRuleNode condition,
        IRuleNode thenNode,
        double threshold = 0.5,
        double elseScore = 0.0)
    {
        _condition = condition;
        _thenNode = thenNode;
        _threshold = threshold;
        _elseScore = elseScore;
    }

    public double Evaluate(UserCategoryProjection projection)
    {
        double conditionResult = _condition.Evaluate(projection);

        return conditionResult >= _threshold
            ? _thenNode.Evaluate(projection)
            : _elseScore;
    }
}
