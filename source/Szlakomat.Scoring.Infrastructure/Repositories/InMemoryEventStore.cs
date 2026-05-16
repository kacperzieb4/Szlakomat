using System.Collections.Concurrent;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Infrastructure.Repositories;

public class InMemoryEventStore : IEventStore
{
    private readonly ConcurrentBag<UserEvent> _events = new();
    
    public Task SaveAsync(UserEvent userEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserEvent>> GetForUserCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserEvent>> GetForUserCategorySinceAsync(Guid userId, string category, DateTime since, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
