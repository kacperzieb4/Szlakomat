using System;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Application.Projections;

public interface IProjectionBuilder
{
    Task RebuildAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default);
}
