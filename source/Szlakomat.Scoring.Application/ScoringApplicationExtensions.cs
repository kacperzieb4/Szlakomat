using Microsoft.Extensions.DependencyInjection;
using Szlakomat.Scoring.Application.Projections;
using Szlakomat.Scoring.Application.Services;

namespace Szlakomat.Scoring.Application;

public static class ScoringApplicationExtensions
{
    public static IServiceCollection AddScoringApplication(this IServiceCollection services)
    {
        services.Configure<ProjectionSettings>(_ => { }); // Use default values
        
        services.AddScoped<IProjectionBuilder, UserCategoryProjectionBuilder>();
        services.AddScoped<IProjectionService, ProjectionService>();
        
        return services;
    }
}
