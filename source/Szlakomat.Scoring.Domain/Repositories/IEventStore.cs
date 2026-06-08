using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Domain.Repositories;

public interface IEventStore
{
    Task SaveAsync(UserEvent userEvent, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEvent>> GetForUserCategoryAsync(Guid userId, TrailCategory category, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEvent>> GetForUserCategorySinceAsync(Guid userId, TrailCategory category, DateTime since, CancellationToken cancellationToken = default);
}
