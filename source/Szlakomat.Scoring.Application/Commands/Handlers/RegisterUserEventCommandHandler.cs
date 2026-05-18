using MediatR;
using Szlakomat.Scoring.Application.Mappers;
using Szlakomat.Scoring.Application.Services;

namespace Szlakomat.Scoring.Application.Commands.Handlers;

public class RegisterUserEventCommandHandler
    : IRequestHandler<RegisterUserEventCommand>
{
    private readonly IEventMapper _mapper;

    private readonly IProjectionService _projectionService;

    public RegisterUserEventCommandHandler(
        IEventMapper mapper,
        IProjectionService projectionService)
    {
        _mapper = mapper;
        _projectionService = projectionService;
    }

    public async Task Handle(
        RegisterUserEventCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = _mapper.Map(request);

        await _projectionService.Apply(domainEvent);
    }
}