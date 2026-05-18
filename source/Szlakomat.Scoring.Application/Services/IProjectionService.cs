using Szlakomat.Scoring.Domain.Events;

namespace Szlakomat.Scoring.Application.Services;

public interface IProjectionService
{
    Task Apply(UserEvent evt);
}