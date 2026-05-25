using System;
using System.Threading.Tasks;
using Szlakomat.Scoring.Domain.Events;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Repositories;

namespace Szlakomat.Scoring.Application.Services;

public class ProjectionService : IProjectionService
{
    private readonly IProjectionRepository _projectionRepository;

    public ProjectionService(IProjectionRepository projectionRepository)
    {
        _projectionRepository = projectionRepository;
    }

    public async Task Apply(UserEvent evt)
    {
        if (evt == null)
        {
            return;
        }

        var projection = await _projectionRepository.GetAsync(evt.UserId, evt.Category) 
            ?? new UserCategoryProjection 
            { 
                UserId = evt.UserId, 
                Category = evt.Category,
                Clicks30Days = 0,
                Purchases90Days = 0,
                Skips30Days = 0,
                AverageRating = 0.0
            };

        switch (evt.Type)
        {
            case EventType.Click:
                projection.Clicks30Days++;
                break;
            case EventType.Purchase:
                projection.Purchases90Days++;
                break;
            case EventType.Skip:
                projection.Skips30Days++;
                break;
        }

        await _projectionRepository.SaveAsync(projection);
    }
}
