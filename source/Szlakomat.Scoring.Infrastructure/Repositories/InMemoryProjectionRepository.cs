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
        throw new NotImplementedException();
    }

    public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
