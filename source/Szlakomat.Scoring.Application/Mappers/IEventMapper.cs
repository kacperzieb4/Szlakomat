using Szlakomat.Scoring.Application.DTO;
using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Mappers;

public interface IEventMapper
{
    UserEvent Map(RegisterUserEventRequest request);
}