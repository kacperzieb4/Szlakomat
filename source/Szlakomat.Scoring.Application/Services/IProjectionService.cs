using System;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Services;

public interface IProjectionService
{
    Task ApplyAsync(UserEvent evt, CancellationToken cancellationToken = default);
    Task RebuildProjectionAsync(Guid userId, string category, CancellationToken cancellationToken = default);
}
