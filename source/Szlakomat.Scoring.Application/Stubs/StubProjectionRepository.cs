using System;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Application.Stubs;

public class StubProjectionRepository : IProjectionRepository
{
    public Task<UserCategoryProjection> GetAsync(Guid userId, string category)
    {
        return Task.FromResult(new UserCategoryProjection
        {
            UserId = userId,
            Category = category,
            Clicks30Days = 8,
            Purchases90Days = 3,
            Skips30Days = 2,
            AverageRating = 4.0
        });
    }
}
