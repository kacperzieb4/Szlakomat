using Szlakomat.Scoring.Domain.Fuzzy;

namespace Szlakomat.Scoring.Domain.Tests;

public class ScoreNormalizerTests
{
    [Theory]
    [InlineData(-50.0, 0.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(50.0, 0.5)]
    [InlineData(100.0, 1.0)]
    [InlineData(150.0, 1.0)]
    public void Normalize_ShouldScaleAndClampCorrectly_WithDefaultMaxScore(double rawScore, double expected)
    {
        // Arrange
        var normalizer = new ScoreNormalizer(100.0);

        // Act
        var result = normalizer.Normalize(rawScore);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Normalize_ShouldScaleCorrectly_WithCustomMaxScore()
    {
        // Arrange
        var normalizer = new ScoreNormalizer(200.0);

        // Act
        var result = normalizer.Normalize(100.0);

        // Assert
        result.Should().BeApproximately(0.5, 0.0001);
    }
}
