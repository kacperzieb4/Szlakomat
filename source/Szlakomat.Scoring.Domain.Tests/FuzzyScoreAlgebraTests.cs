using Szlakomat.Scoring.Domain.Fuzzy;

namespace Szlakomat.Scoring.Domain.Tests;

public class FuzzyScoreAlgebraTests
{
    private readonly FuzzyScoreAlgebra _algebra = new();

    [Fact]
    public void And_ShouldReturnMinimumValue_WhenMultipleValuesArePassed()
    {
        // Arrange
        var values = new[] { 0.8, 0.5, 0.9 };

        // Act
        var result = _algebra.And(values);

        // Assert
        result.Should().BeApproximately(0.5, 0.0001);
    }

    [Fact]
    public void And_EmptyCollection_ShouldReturnZero()
    {
        // Act & Assert
        _algebra.And([]).Should().BeApproximately(0.0, 0.0001);
    }

    [Fact]
    public void Or_ShouldReturnMaximumValue_WhenMultipleValuesArePassed()
    {
        // Arrange
        var values = new[] { 0.8, 0.5, 0.9 };

        // Act
        var result = _algebra.Or(values);

        // Assert
        result.Should().BeApproximately(0.9, 0.0001);
    }

    [Fact]
    public void Or_EmptyCollection_ShouldReturnZero()
    {
        // Act & Assert
        _algebra.Or([]).Should().BeApproximately(0.0, 0.0001);
    }

    [Fact]
    public void Not_ShouldInvertValue_WhenValidFuzzyValueIsPassed()
    {
        // Arrange
        double value = 0.3;

        // Act
        var result = _algebra.Not(value);

        // Assert
        result.Should().BeApproximately(0.7, 0.0001);
    }

    [Fact]
    public void Not_Zero_ShouldReturnOne()
    {
        // Act & Assert
        _algebra.Not(0.0).Should().BeApproximately(1.0, 0.0001);
    }

    [Fact]
    public void Not_One_ShouldReturnZero()
    {
        // Act & Assert
        _algebra.Not(1.0).Should().BeApproximately(0.0, 0.0001);
    }
}
