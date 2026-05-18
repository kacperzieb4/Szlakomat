using Szlakomat.Scoring.Application.Commands;
using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Mappers;

public interface IEventMapper
{
    UserEvent Map(RegisterUserEventCommand command);
}