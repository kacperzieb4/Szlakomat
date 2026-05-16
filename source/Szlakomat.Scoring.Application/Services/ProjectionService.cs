using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Application.Services;

public class ProjectionService : IProjectionService
{
    private readonly IEventStore _eventStore;
    private readonly IProjectionRepository _projectionRepository;

    public ProjectionService(IEventStore eventStore, IProjectionRepository projectionRepository)
    {
        _eventStore = eventStore;
        _projectionRepository = projectionRepository;
    }

    public Task ApplyAsync(UserEvent evt, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RebuildProjectionAsync(Guid userId, string category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
