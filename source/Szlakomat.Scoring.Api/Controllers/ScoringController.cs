using MediatR;
using Microsoft.AspNetCore.Mvc;
using Szlakomat.Scoring.Application.Commands;
using Szlakomat.Scoring.Application.DTO;
using Szlakomat.Scoring.Application.Queries;

namespace Szlakomat.Scoring.Api.Controllers;

[ApiController]
[Route("api/scoring")]
public class ScoringController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScoringController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("events")]
    public async Task<IActionResult> RegisterEvent(
        RegisterUserEventRequest request)
    {
        var command = new RegisterUserEventCommand(
            request.UserId,
            request.Category,
            request.EventType
        );

        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("{userId}/{category}")]
    public async Task<IActionResult> GetScore(
        Guid userId,
        string category)
    {
        var query = new GetUserScoreQuery(
            userId,
            category
        );

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}