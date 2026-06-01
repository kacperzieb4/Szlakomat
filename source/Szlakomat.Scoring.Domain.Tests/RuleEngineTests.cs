using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Fuzzy.Nodes;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;
using Szlakomat.Scoring.Domain.Rules.Composite;

namespace Szlakomat.Scoring.Domain.Tests;

public class RuleEngineTests
{
    private readonly FuzzyScoreAlgebra _algebra = new();
    private readonly Guid _userId = Guid.NewGuid();
    private const string Category = "Szlaki Górskie";

    private UserCategoryProjection CreateProjection(
        int clicks = 5, int purchases = 3, int skips = 2, double rating = 4.0)
    {
        return new UserCategoryProjection
        {
            UserId = _userId,
            Category = Category,
            Clicks30Days = clicks,
            Purchases90Days = purchases,
            Skips30Days = skips,
            AverageRating = rating
        };
    }

    // ===================== AndNode =====================

    [Fact]
    public void AndNode_ShouldReturnMinimum_WhenMultipleChildrenProvided()
    {
        // Arrange
        var projection = CreateProjection(clicks: 8, purchases: 3);
        var node = new AndNode(_algebra, new List<IRuleNode>
        {
            new FuzzyClicksNode(10),    // 8/10 = 0.8
            new FuzzyPurchasesNode(5)   // 3/5  = 0.6
        });

        // Act
        var result = node.Evaluate(projection);

        // Assert — AND = min(0.8, 0.6) = 0.6
        result.Should().BeApproximately(0.6, 0.0001);
    }

    [Fact]
    public void AndNode_EmptyChildren_ShouldReturnZero()
    {
        // Arrange
        var node = new AndNode(_algebra, new List<IRuleNode>());
        var projection = CreateProjection();

        // Act & Assert
        node.Evaluate(projection).Should().BeApproximately(0.0, 0.0001);
    }

    // ===================== OrNode =====================

    [Fact]
    public void OrNode_ShouldReturnMaximum_WhenMultipleChildrenProvided()
    {
        // Arrange
        var projection = CreateProjection(clicks: 8, purchases: 3);
        var node = new OrNode(_algebra, new List<IRuleNode>
        {
            new FuzzyClicksNode(10),    // 0.8
            new FuzzyPurchasesNode(5)   // 0.6
        });

        // Act
        var result = node.Evaluate(projection);

        // Assert — OR = max(0.8, 0.6) = 0.8
        result.Should().BeApproximately(0.8, 0.0001);
    }

    // ===================== NotNode =====================

    [Fact]
    public void NotNode_ShouldInvertChildScore()
    {
        // Arrange
        var projection = CreateProjection(clicks: 8); // clicks = 0.8
        var node = new NotNode(_algebra, new FuzzyClicksNode(10));

        // Act
        var result = node.Evaluate(projection);

        // Assert — NOT(0.8) = 0.2
        result.Should().BeApproximately(0.2, 0.0001);
    }

    // ===================== IfThenNode =====================

    [Fact]
    public void IfThenNode_WhenConditionMet_ShouldReturnThenScore()
    {
        // Arrange — clicks = 8/10 = 0.8 >= threshold 0.5
        var projection = CreateProjection(clicks: 8, rating: 4.0);
        var node = new IfThenNode(
            condition: new FuzzyClicksNode(10),
            thenNode: new FuzzyRatingNode(),
            threshold: 0.5,
            elseScore: 0.0
        );

        // Act
        var result = node.Evaluate(projection);

        // Assert — condition met (0.8 >= 0.5), return rating = 4.0/5.0 = 0.8
        result.Should().BeApproximately(0.8, 0.0001);
    }

    [Fact]
    public void IfThenNode_WhenConditionNotMet_ShouldReturnElseScore()
    {
        // Arrange — clicks = 2/10 = 0.2 < threshold 0.5
        var projection = CreateProjection(clicks: 2, rating: 4.0);
        var node = new IfThenNode(
            condition: new FuzzyClicksNode(10),
            thenNode: new FuzzyRatingNode(),
            threshold: 0.5,
            elseScore: 0.0
        );

        // Act
        var result = node.Evaluate(projection);

        // Assert — condition NOT met, return elseScore = 0.0
        result.Should().BeApproximately(0.0, 0.0001);
    }

    // ===================== WeightedNode =====================

    [Fact]
    public void WeightedNode_ShouldCalculateWeightedAverage()
    {
        // Arrange
        var projection = CreateProjection(clicks: 10, purchases: 5);
        var node = new WeightedNode(new List<(IRuleNode, double)>
        {
            (new FuzzyClicksNode(10), 2.0),    // 1.0 * 2.0 = 2.0
            (new FuzzyPurchasesNode(5), 3.0)   // 1.0 * 3.0 = 3.0
        });

        // Act
        var result = node.Evaluate(projection);

        // Assert — (2.0 + 3.0) / (2.0 + 3.0) = 1.0
        result.Should().BeApproximately(1.0, 0.0001);
    }

    [Fact]
    public void WeightedNode_ShouldRespectWeights()
    {
        // Arrange — clicks=10/10=1.0, purchases=0/5=0.0
        var projection = CreateProjection(clicks: 10, purchases: 0);
        var node = new WeightedNode(new List<(IRuleNode, double)>
        {
            (new FuzzyClicksNode(10), 1.0),    // 1.0 * 1.0 = 1.0
            (new FuzzyPurchasesNode(5), 1.0)   // 0.0 * 1.0 = 0.0
        });

        // Act
        var result = node.Evaluate(projection);

        // Assert — (1.0 + 0.0) / (1.0 + 1.0) = 0.5
        result.Should().BeApproximately(0.5, 0.0001);
    }

    [Fact]
    public void WeightedNode_EmptyChildren_ShouldReturnZero()
    {
        // Arrange
        var node = new WeightedNode(new List<(IRuleNode, double)>());
        var projection = CreateProjection();

        // Act & Assert
        node.Evaluate(projection).Should().BeApproximately(0.0, 0.0001);
    }

    // ===================== Kompozycja zagnieżdżona =====================

    [Fact]
    public void NestedComposite_ShouldEvaluateCorrectly()
    {
        // Arrange — zagnieżdżone drzewo:
        //   And(
        //     Or(clicks, purchases),
        //     Not(skips_inverted)
        //   )
        var projection = CreateProjection(clicks: 8, purchases: 3, skips: 2);

        var orNode = new OrNode(_algebra, new List<IRuleNode>
        {
            new FuzzyClicksNode(10),    // 0.8
            new FuzzyPurchasesNode(5)   // 0.6
        }); // OR = 0.8

        // FuzzySkipsNode już robi 1.0 - skips/max, więc Not odwróci to z powrotem
        var notNode = new NotNode(_algebra, new FuzzySkipsNode(10)); // Not(0.8) = 0.2

        var andNode = new AndNode(_algebra, new List<IRuleNode> { orNode, notNode });

        // Act
        var result = andNode.Evaluate(projection);

        // Assert — And(0.8, 0.2) = min = 0.2
        result.Should().BeApproximately(0.2, 0.0001);
    }

    // ===================== RuleTreeBuilder =====================

    [Fact]
    public void RuleTreeBuilder_ShouldBuildAndNode()
    {
        // Arrange
        var builder = new RuleTreeBuilder(_algebra);
        var projection = CreateProjection(clicks: 10, purchases: 5);

        // Act
        var tree = builder.And(
            new FuzzyClicksNode(10),
            new FuzzyPurchasesNode(5)
        );
        var result = tree.Evaluate(projection);

        // Assert — And(1.0, 1.0) = 1.0
        result.Should().BeApproximately(1.0, 0.0001);
    }

    [Fact]
    public void RuleTreeBuilder_ShouldBuildWeightedNode()
    {
        // Arrange
        var builder = new RuleTreeBuilder(_algebra);
        var projection = CreateProjection(clicks: 5, purchases: 5);

        // Act
        var tree = builder.Weighted(
            (new FuzzyClicksNode(10), 1.0),  // 0.5 * 1.0
            (new FuzzyPurchasesNode(5), 1.0) // 1.0 * 1.0
        );
        var result = tree.Evaluate(projection);

        // Assert — (0.5 + 1.0) / 2.0 = 0.75
        result.Should().BeApproximately(0.75, 0.0001);
    }

    // ===================== DefaultRuleTreeFactory =====================

    [Fact]
    public void DefaultRuleTreeFactory_ShouldCreateWorkingTree()
    {
        // Arrange
        var tree = DefaultRuleTreeFactory.Create(_algebra);
        var projection = CreateProjection(clicks: 8, purchases: 3, skips: 2, rating: 4.0);

        // Act
        var result = tree.Evaluate(projection);

        // Assert — powinien zwrócić wartość z przedziału [0, 1]
        result.Should().BeGreaterOrEqualTo(0.0);
        result.Should().BeLessOrEqualTo(1.0);
    }

    [Fact]
    public void DefaultRuleTreeFactory_ShouldReturnHighScore_ForEngagedUser()
    {
        // Arrange
        var tree = DefaultRuleTreeFactory.Create(_algebra);
        var projection = CreateProjection(clicks: 10, purchases: 5, skips: 0, rating: 5.0);

        // Act
        var result = tree.Evaluate(projection);

        // Assert — maksymalnie zaangażowany użytkownik powinien mieć wysoki wynik
        result.Should().BeApproximately(1.0, 0.0001);
    }

    [Fact]
    public void DefaultRuleTreeFactory_ShouldReturnLowScore_ForInactiveUser()
    {
        // Arrange
        var tree = DefaultRuleTreeFactory.Create(_algebra);
        var projection = CreateProjection(clicks: 0, purchases: 0, skips: 10, rating: 0.0);

        // Act
        var result = tree.Evaluate(projection);

        // Assert — nieaktywny użytkownik powinien mieć niski wynik
        result.Should().BeLessThan(0.2);
    }
}
