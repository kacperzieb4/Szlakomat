using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Application.Services;

public class ProjectionService : IProjectionService
{
    private readonly IEventStore _eventStore;
    private readonly IProjectionRepository _projectionRepository;

    public ProjectionService(IEventStore eventStore, IProjectionRepository projectionRepository)
    {
        _eventStore = eventStore;
        _projectionRepository = projectionRepository;
    }

    public async Task ApplyAsync(UserEvent evt, CancellationToken cancellationToken = default)
    {
        await _eventStore.SaveAsync(evt, cancellationToken);
        await RebuildProjectionAsync(evt.UserId, evt.Category, cancellationToken);
    }

    public async Task RebuildProjectionAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var events90Days = await _eventStore.GetForUserCategorySinceAsync(userId, category, now.AddDays(-90), cancellationToken);
        var events30Days = events90Days.Where(e => e.OccurredAt >= now.AddDays(-30)).ToList();
        
        var projection = new UserCategoryProjection
        {
            UserId = userId,
            Category = category,
            Clicks30Days = events30Days.Count(e => e.Type == EventType.Click),
            Skips30Days = events30Days.Count(e => e.Type == EventType.Skip),
            Purchases90Days = events90Days.Count(e => e.Type == EventType.Purchase)
        };
        
        // approximate rating using HighRating and LowRating.
        var highRatings = events90Days.Count(e => e.Type == EventType.HighRating);
        var lowRatings = events90Days.Count(e => e.Type == EventType.LowRating);
        
        var totalRatings = highRatings + lowRatings;
        projection.AverageRating = totalRatings > 0 
            ? ((highRatings * 5.0) + (lowRatings * 1.0)) / totalRatings 
            : 0.0;
            
        await _projectionRepository.SaveAsync(projection, cancellationToken);
    }
}
