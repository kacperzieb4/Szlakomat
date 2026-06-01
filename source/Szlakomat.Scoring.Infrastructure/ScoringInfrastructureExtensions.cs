using Microsoft.Extensions.DependencyInjection;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Infrastructure.Repositories;

namespace Szlakomat.Scoring.Infrastructure;

public static class ScoringInfrastructureExtensions
{
    public static IServiceCollection AddScoringInfrastructure(this IServiceCollection services)
    {
        // InMemory repositories must be registered as Singletons so state persists across requests
        services.AddSingleton<IEventStore, InMemoryEventStore>();
        services.AddSingleton<IProjectionRepository, InMemoryProjectionRepository>();
        
        return services;
    }
}
