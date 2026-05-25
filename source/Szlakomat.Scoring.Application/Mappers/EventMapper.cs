using Szlakomat.Scoring.Application.DTO;
using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Mappers;

public class EventMapper : IEventMapper
{
    public UserEvent Map(RegisterUserEventRequest request)
    {
        return new UserEvent
        {
            UserId = request.UserId,
            Category = request.Category,
            Type = Enum.Parse<EventType>(request.EventType),
            OccurredAt = DateTime.UtcNow
        };
    }
}