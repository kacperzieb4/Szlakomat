using System;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Application.Stubs;

public class StubProjectionRepository : IProjectionRepository
{
    public Task<UserCategoryProjection?> GetAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<UserCategoryProjection?>(new UserCategoryProjection
        {
            UserId = userId,
            Category = category,
            Clicks30Days = 8,
            Purchases90Days = 3,
            Skips30Days = 2,
            AverageRating = 4.0
        });
    }

    public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
