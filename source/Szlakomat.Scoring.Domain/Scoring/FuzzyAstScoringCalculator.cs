using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;

namespace Szlakomat.Scoring.Domain.Scoring;

public class FuzzyAstScoringCalculator : IScoringCalculator
{
    private readonly IRuleNode _ruleTree;
    private readonly IScoreNormalizer _normalizer;

    public FuzzyAstScoringCalculator(IRuleNode ruleTree, IScoreNormalizer normalizer)
    {
        _ruleTree = ruleTree;
        _normalizer = normalizer;
    }

    public double CalculateScore(UserCategoryProjection projection)
    {
        if (projection == null)
        {
            return 0.0;
        }

        // Evaluate the AST rule tree
        double rawScore = _ruleTree.Evaluate(projection);

        // Normalize the score using fuzzy logic normalization
        return _normalizer.Normalize(rawScore);
    }
}
