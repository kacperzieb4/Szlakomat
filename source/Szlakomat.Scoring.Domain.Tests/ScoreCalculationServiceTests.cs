using System;
using System.Threading.Tasks;
using FluentAssertions;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Application.Stubs;
using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.ValueObjects;
using Xunit;

namespace Szlakomat.Scoring.Domain.Tests;

public class ScoreCalculationServiceTests
{
    [Fact]
    public async Task CalculateFinalScoreAsync_ShouldReturnValidScoreResult_WhenStubsAreUsed()
    {
        // Arrange
        var algebra = new FuzzyScoreAlgebra();
        var normalizer = new ScoreNormalizer(100.0); // 80.0 raw score -> 0.8 normalized score
        var projections = new StubProjectionRepository();
        var ruleTree = new StubRuleNode();

        var service = new ScoreCalculationService(algebra, normalizer, projections, ruleTree);
        var userId = Guid.NewGuid();
        var category = new TrailCategory("Szlaki Górskie");

        // Act
        var result = await service.CalculateFinalScoreAsync(userId, category);

        // Assert
        result.Should().NotBeNull();
        result.Score.Should().BeApproximately(0.8, 0.0001);
        result.Explanations.Should().HaveCount(4);

        // Weryfikacja poprawności generowania dynamicznych wyjaśnień i wkładu (Contribution) każdego węzła rozmytego
        result.Explanations[0].Reason.Should().Contain("Poziom aktywności kliknięć");
        result.Explanations[0].Contribution.Should().BeApproximately(0.8, 0.0001); // 8/10 kliknięć

        result.Explanations[1].Reason.Should().Contain("Zaangażowanie finansowe");
        result.Explanations[1].Contribution.Should().BeApproximately(0.6, 0.0001); // 3/5 zakupów

        result.Explanations[2].Reason.Should().Contain("Średnia ocena satysfakcji");
        result.Explanations[2].Contribution.Should().BeApproximately(0.8, 0.0001); // 4.0/5.0 średnia ocena

        result.Explanations[3].Reason.Should().Contain("Wskaźnik braku znudzenia");
        result.Explanations[3].Contribution.Should().BeApproximately(0.8, 0.0001); // 1.0 - 2/10 pominięć
    }
}
