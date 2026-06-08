using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Szlakomat.Scoring.Application.Projections;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Application.Services;

public class ProjectionService : IProjectionService
{
    private readonly IEventStore _eventStore;
    private readonly IEnumerable<IProjectionBuilder> _builders;

    public ProjectionService(IEventStore eventStore, IEnumerable<IProjectionBuilder> builders)
    {
        _eventStore = eventStore;
        _builders = builders;
    }

    public async Task ApplyAsync(UserEvent evt, CancellationToken cancellationToken = default)
    {
        await _eventStore.SaveAsync(evt, cancellationToken);
        
        var tasks = _builders.Select(b => b.RebuildAsync(evt.UserId, evt.Category, cancellationToken));
        await Task.WhenAll(tasks);
    }
}
