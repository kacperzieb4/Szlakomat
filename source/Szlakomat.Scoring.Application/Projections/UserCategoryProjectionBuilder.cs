using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Application.Projections;

public class UserCategoryProjectionBuilder : IProjectionBuilder
{
    private readonly IEventStore _eventStore;
    private readonly IProjectionRepository _projectionRepository;
    private readonly ProjectionSettings _settings;

    public UserCategoryProjectionBuilder(
        IEventStore eventStore, 
        IProjectionRepository projectionRepository,
        IOptions<ProjectionSettings> options)
    {
        _eventStore = eventStore;
        _projectionRepository = projectionRepository;
        _settings = options.Value;
    }

    public async Task RebuildAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var eventsHistory = await _eventStore.GetForUserCategorySinceAsync(
            userId, category, now.AddDays(-_settings.HistoryActivityWindowDays), cancellationToken);
            
        var projection = new UserCategoryProjection(userId, category);

        foreach (var evt in eventsHistory)
        {
            bool isRecent = evt.OccurredAt >= now.AddDays(-_settings.RecentActivityWindowDays);
            projection.Apply(evt, isRecent);
        }
            
        await _projectionRepository.SaveAsync(projection, cancellationToken);
    }
}
