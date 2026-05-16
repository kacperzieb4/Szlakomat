using System.Collections.Concurrent;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Infrastructure.Repositories;

public class InMemoryProjectionRepository : IProjectionRepository
{
    private readonly ConcurrentDictionary<string, UserCategoryProjection> _projections = new();
    
    private static string GetKey(Guid userId, string category) => $"{userId}:{category}";

    public Task<UserCategoryProjection?> GetAsync(Guid userId, string category, CancellationToken cancellationToken = default)
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
