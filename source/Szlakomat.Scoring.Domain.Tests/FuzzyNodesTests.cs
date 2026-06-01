using System;
using Szlakomat.Scoring.Domain.Fuzzy.Nodes;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Tests;

public class FuzzyNodesTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private const string Category = "Szlaki Górskie";

    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(5, 0.5)]
    [InlineData(10, 1.0)]
    [InlineData(15, 1.0)]
    public void FuzzyClicksNode_ShouldEvaluateCorrectly(int clicks, double expected)
    {
        // Arrange
        var node = new FuzzyClicksNode(10);
        var projection = new UserCategoryProjection
        {
            UserId = _userId,
            Category = Category,
            Clicks30Days = clicks
        };

        // Act
        var result = node.Evaluate(projection);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(3, 0.6)]
    [InlineData(5, 1.0)]
    [InlineData(8, 1.0)]
    public void FuzzyPurchasesNode_ShouldEvaluateCorrectly(int purchases, double expected)
    {
        // Arrange
        var node = new FuzzyPurchasesNode(5);
        var projection = new UserCategoryProjection
        {
            UserId = _userId,
            Category = Category,
            Purchases90Days = purchases
        };

        // Act
        var result = node.Evaluate(projection);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(2.5, 0.5)]
    [InlineData(5.0, 1.0)]
    public void FuzzyRatingNode_ShouldEvaluateCorrectly(double rating, double expected)
    {
        // Arrange
        var node = new FuzzyRatingNode();
        var projection = new UserCategoryProjection
        {
            UserId = _userId,
            Category = Category,
            AverageRating = rating
        };

        // Act
        var result = node.Evaluate(projection);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData(0, 1.0)]
    [InlineData(2, 0.8)]
    [InlineData(10, 0.0)]
    [InlineData(15, 0.0)]
    public void FuzzySkipsNode_ShouldEvaluateCorrectly(int skips, double expected)
    {
        // Arrange
        var node = new FuzzySkipsNode(10);
        var projection = new UserCategoryProjection
        {
            UserId = _userId,
            Category = Category,
            Skips30Days = skips
        };

        // Act
        var result = node.Evaluate(projection);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }
}
