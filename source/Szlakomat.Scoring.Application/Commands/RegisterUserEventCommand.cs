using MediatR;

namespace Szlakomat.Scoring.Application.Commands;

public record RegisterUserEventCommand(
    Guid UserId,
    string Category,
    string EventType
) : IRequest;