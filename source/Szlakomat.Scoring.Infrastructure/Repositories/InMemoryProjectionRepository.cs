using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Infrastructure.Repositories;

public class InMemoryProjectionRepository : IProjectionRepository
{
    private readonly ConcurrentDictionary<string, UserCategoryProjection> _projections = new();
    
    private static string GetKey(Guid userId, TrailCategory category) => $"{userId}:{category.Value}";

    public Task<UserCategoryProjection?> GetAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default)
    {
        _projections.TryGetValue(GetKey(userId, category), out var projection);
        return Task.FromResult(projection);
    }

    public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
    {
        _projections[GetKey(projection.UserId, projection.Category)] = projection;
        return Task.CompletedTask;
    }
}
