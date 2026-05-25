using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Infrastructure.Repositories;

public class InMemoryProjectionRepository : IProjectionRepository
{
    private readonly ConcurrentDictionary<(Guid UserId, string Category), UserCategoryProjection> _projections = new();

    public Task<UserCategoryProjection?> GetAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        _projections.TryGetValue((userId, category), out var projection);
        return Task.FromResult(projection);
    }

    public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
    {
        _projections[(projection.UserId, projection.Category)] = projection;
        return Task.CompletedTask;
    }
}
