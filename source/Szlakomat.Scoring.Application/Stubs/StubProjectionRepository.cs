using System;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Application.Stubs;

public class StubProjectionRepository : IProjectionRepository
{
    public Task<UserCategoryProjection?> GetAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default)
    {
        var projection = UserCategoryProjection.Restore(
            userId: userId, 
            category: category, 
            recentClicks: 8, 
            historyPurchases: 3, 
            recentSkips: 2, 
            averageRating: 4.0);
            
        return Task.FromResult<UserCategoryProjection?>(projection);
    }

    public Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
