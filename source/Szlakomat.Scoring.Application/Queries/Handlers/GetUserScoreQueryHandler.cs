using MediatR;
using Szlakomat.Scoring.Application.DTO;
using Szlakomat.Scoring.Application.Services;

namespace Szlakomat.Scoring.Application.Queries.Handlers;

public class GetUserScoreQueryHandler
    : IRequestHandler<GetUserScoreQuery, ScoreResponse>
{
    private readonly IScoreService _scoreService;

    public GetUserScoreQueryHandler(
        IScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    public async Task<ScoreResponse> Handle(
        GetUserScoreQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _scoreService.Calculate(
            request.UserId,
            request.Category);

        return new ScoreResponse
        {
            Score = result.Score,
            Reasons = result.Reasons
        };
    }
}