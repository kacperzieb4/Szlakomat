using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.Scoring;
using Szlakomat.Scoring.Domain.ValueObjects;
using Xunit;

namespace Szlakomat.Scoring.Domain.Tests;

public class ScoreServiceTests
{
    private readonly FakeProjectionRepository _repository = new();
    private readonly IScoringCalculator _calculator = new DefaultScoringCalculator();
    private readonly ScoreService _service;

    public ScoreServiceTests()
    {
        _service = new ScoreService(_calculator, _repository);
    }

    [Fact]
    public async Task Calculate_ShouldReturnZeroAndWarning_WhenNoProjectionExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var category = new TrailCategory("Hiking");
        _repository.Projection = null;

        // Act
        var result = await _service.Calculate(userId, category);

        // Assert
        result.Should().NotBeNull();
        result.Score.Should().Be(0.0);
        result.Reasons.Should().Contain("No scoring history found for this category.");
    }

    [Fact]
    public async Task Calculate_ShouldReturnScoreAndDetails_WhenProjectionExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var category = new TrailCategory("Hiking");
        
        var projection = UserCategoryProjection.Restore(
            userId: userId, 
            category: category, 
            recentClicks: 10, 
            historyPurchases: 1, 
            recentSkips: 2, 
            averageRating: 5.0);
        
        _repository.Projection = projection;

        // Act
        var result = await _service.Calculate(userId, category);

        // Assert
        // Expected raw score: (10 * 0.05) + (1 * 0.20) - (2 * 0.10) = 0.50 + 0.20 - 0.20 = 0.50
        result.Should().NotBeNull();
        result.Score.Should().BeApproximately(0.50, 0.001);
        result.Reasons.Should().HaveCount(2);
        result.Reasons[0].Should().Contain("10 recent clicks");
        result.Reasons[0].Should().Contain("1 historical purchases");
        result.Reasons[0].Should().Contain("2 recent skips");
        result.Reasons[1].Should().Contain("5.00"); // 4 HighRatings = 5.0 avg
    }

    private class FakeProjectionRepository : IProjectionRepository
    {
        public UserCategoryProjection? Projection { get; set; }

        public Task<UserCategoryProjection?> GetAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Projection);
        }

        public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
        {
            Projection = projection;
            return Task.CompletedTask;
        }
    }
}
