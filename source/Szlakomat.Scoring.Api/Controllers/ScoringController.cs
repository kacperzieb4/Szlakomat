using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Szlakomat.Scoring.Application.DTO;
using Szlakomat.Scoring.Application.Mappers;
using Szlakomat.Scoring.Application.Services;

namespace Szlakomat.Scoring.Api.Controllers;

[ApiController]
[Route("api/scoring")]
public class ScoringController : ControllerBase
{
    private readonly IProjectionService _projectionService;
    private readonly IScoreService _scoreService;
    private readonly IEventMapper _eventMapper;
    private readonly IValidator<RegisterUserEventRequest> _validator;

    public ScoringController(
        IProjectionService projectionService,
        IScoreService scoreService,
        IEventMapper eventMapper,
        IValidator<RegisterUserEventRequest> validator)
    {
        _projectionService = projectionService;
        _scoreService = scoreService;
        _eventMapper = eventMapper;
        _validator = validator;
    }

    [HttpPost("events")]
    public async Task<IActionResult> RegisterEvent(
        [FromBody] RegisterUserEventRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var domainEvent = _eventMapper.Map(request);
        await _projectionService.ApplyAsync(domainEvent);

        return Ok();
    }

    [HttpGet("{userId}/{category}")]
    public async Task<IActionResult> GetScore(
        Guid userId,
        string category)
    {
        var result = await _scoreService.Calculate(userId, category);

        var response = new ScoreResponse
        {
            Score = result.Score,
            Reasons = result.Reasons
        };

        return Ok(response);
    }
}