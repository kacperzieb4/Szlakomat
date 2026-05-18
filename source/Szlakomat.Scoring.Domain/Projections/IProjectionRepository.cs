using System;
using System.Threading.Tasks;

namespace Szlakomat.Scoring.Domain.Projections;

public interface IProjectionRepository
{
    Task<UserCategoryProjection> GetAsync(Guid userId, string category);
}
