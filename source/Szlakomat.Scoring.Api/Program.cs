using FluentValidation;
using Szlakomat.Scoring.Application;
using Szlakomat.Scoring.Application.Mappers;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Application.Validators;
using Szlakomat.Scoring.Domain.Scoring;
using Szlakomat.Scoring.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Register clean/simplified scoring components
builder.Services.AddScoringApplication();
builder.Services.AddScoringInfrastructure();

builder.Services.AddScoped<IEventMapper, EventMapper>();
builder.Services.AddScoped<IScoringCalculator, DefaultScoringCalculator>();
builder.Services.AddScoped<IScoreService, ScoreService>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserEventRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();