using System;
using Szlakomat.Scoring.Domain.ValueObjects;

namespace Szlakomat.Scoring.Domain.Events;

public class UserEvent
{
    public Guid UserId { get; set; }

    public required TrailCategory Category { get; set; }

    public EventType Type { get; set; }

    public DateTime OccurredAt { get; set; }
}
