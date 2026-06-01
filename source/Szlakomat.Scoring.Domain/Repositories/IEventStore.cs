using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Domain.Repositories;

public interface IEventStore
{
    Task SaveAsync(UserEvent userEvent, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEvent>> GetForUserCategoryAsync(Guid userId, string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEvent>> GetForUserCategorySinceAsync(Guid userId, string category, DateTime since, CancellationToken cancellationToken = default);
}
