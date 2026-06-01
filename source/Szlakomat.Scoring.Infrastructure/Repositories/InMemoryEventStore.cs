using System.Collections.Concurrent;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Infrastructure.Repositories;

public class InMemoryEventStore : IEventStore
{
    private readonly ConcurrentBag<UserEvent> _events = new();
    
    public Task SaveAsync(UserEvent userEvent, CancellationToken cancellationToken = default)
    {
        _events.Add(userEvent);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<UserEvent>> GetForUserCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        var userEvents = _events
            .Where(e => e.UserId == userId && e.Category == category)
            .OrderBy(e => e.OccurredAt)
            .AsEnumerable();
            
        return Task.FromResult(userEvents);
    }

    public Task<IEnumerable<UserEvent>> GetForUserCategorySinceAsync(Guid userId, string category, DateTime since, CancellationToken cancellationToken = default)
    {
        var userEvents = _events
            .Where(e => e.UserId == userId && e.Category == category && e.OccurredAt >= since)
            .OrderBy(e => e.OccurredAt)
            .AsEnumerable();
            
        return Task.FromResult(userEvents);
    }
}
