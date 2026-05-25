using FluentValidation;
using Szlakomat.Scoring.Application.Mappers;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Application.Validators;
using Szlakomat.Scoring.Domain.Repositories;
using Szlakomat.Scoring.Domain.Scoring;
using Szlakomat.Scoring.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IEventMapper, EventMapper>();
builder.Services.AddScoped<IScoringCalculator, DefaultScoringCalculator>();
builder.Services.AddScoped<IScoreService, ScoreService>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();
builder.Services.AddSingleton<IProjectionRepository, InMemoryProjectionRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserEventRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();