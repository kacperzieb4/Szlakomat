using Szlakomat.Scoring.Application.Commands;
using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Mappers;

public class EventMapper : IEventMapper
{
    public UserEvent Map(RegisterUserEventCommand command)
    {
        return new UserEvent
        {
            UserId = command.UserId,
            Category = command.Category,
            Type = Enum.Parse<EventType>(command.EventType),
            OccurredAt = DateTime.UtcNow
        };
    }
}