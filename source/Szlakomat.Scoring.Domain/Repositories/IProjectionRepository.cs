using System;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Projections;

namespace Szlakomat.Scoring.Domain.Repositories;

public interface IProjectionRepository
{
    Task<UserCategoryProjection?> GetAsync(Guid userId, string category, CancellationToken cancellationToken = default);
    Task SaveAsync(UserCategoryProjection projection, CancellationToken cancellationToken = default);
}
