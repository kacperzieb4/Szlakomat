using FluentAssertions;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Scoring;
using Xunit;

namespace Szlakomat.Scoring.Domain.Tests;

public class DefaultScoringCalculatorTests
{
    private readonly DefaultScoringCalculator _calculator = new();

    [Fact]
    public void CalculateScore_ShouldReturnZero_WhenProjectionIsNull()
    {
        // Act
        var score = _calculator.CalculateScore(null!);

        // Assert
        score.Should().Be(0.0);
    }

    [Fact]
    public void CalculateScore_ShouldApplyWeightsCorrectly()
    {
        // Arrange
        var projection = new UserCategoryProjection
        {
            UserId = Guid.NewGuid(),
            Category = "Trekking",
            Clicks30Days = 4,      // 4 * 0.05 = 0.20
            Purchases90Days = 2,   // 2 * 0.20 = 0.40
            Skips30Days = 1,       // 1 * 0.10 = 0.10
            AverageRating = 4.5
        };

        // Act
        var score = _calculator.CalculateScore(projection);

        // Assert
        // Expected raw: 0.20 + 0.40 - 0.10 = 0.50
        score.Should().BeApproximately(0.50, 0.001);
    }

    [Fact]
    public void CalculateScore_ShouldClampToZero_WhenRawScoreIsNegative()
    {
        // Arrange
        var projection = new UserCategoryProjection
        {
            UserId = Guid.NewGuid(),
            Category = "Running",
            Clicks30Days = 1,      // 1 * 0.05 = 0.05
            Purchases90Days = 0,   // 0
            Skips30Days = 3,       // 3 * 0.10 = 0.30
            AverageRating = 3.0
        };

        // Act
        var score = _calculator.CalculateScore(projection);

        // Assert
        // Expected raw: 0.05 - 0.30 = -0.25 -> clamped to 0.0
        score.Should().Be(0.0);
    }

    [Fact]
    public void CalculateScore_ShouldClampToOne_WhenRawScoreExceedsOne()
    {
        // Arrange
        var projection = new UserCategoryProjection
        {
            UserId = Guid.NewGuid(),
            Category = "Climbing",
            Clicks30Days = 10,     // 10 * 0.05 = 0.50
            Purchases90Days = 5,   // 5 * 0.20 = 1.00
            Skips30Days = 0,       // 0
            AverageRating = 5.0
        };

        // Act
        var score = _calculator.CalculateScore(projection);

        // Assert
        // Expected raw: 0.50 + 1.00 = 1.50 -> clamped to 1.0
        score.Should().Be(1.0);
    }
}
