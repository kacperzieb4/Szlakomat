using Microsoft.Extensions.DependencyInjection;
using Szlakomat.Scoring.Application.Services;

namespace Szlakomat.Scoring.Application;

public static class ScoringApplicationExtensions
{
    public static IServiceCollection AddScoringApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectionService, ProjectionService>();
        
        return services;
    }
}
