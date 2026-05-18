using MediatR;
using Szlakomat.Scoring.Application.DTO;

namespace Szlakomat.Scoring.Application.Queries;

public record GetUserScoreQuery(
    Guid UserId,
    string Category
) : IRequest<ScoreResponse>;