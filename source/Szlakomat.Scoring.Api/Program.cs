using FluentValidation;
using Szlakomat.Scoring.Application;
using Szlakomat.Scoring.Application.Mappers;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Application.Validators;
using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;
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

// Rejestracja zależności modułu scoringowego
builder.Services.AddScoped<IScoreAlgebra, FuzzyScoreAlgebra>();
builder.Services.AddScoped<IScoreNormalizer>(_ => new ScoreNormalizer(maxScore: 100.0));
builder.Services.AddScoped<IScoreCalculationService, ScoreCalculationService>();

// Rejestracja atrap (Stubs) - StubProjectionRepository
builder.Services.AddScoped<IProjectionRepository, Szlakomat.Scoring.Application.Stubs.StubProjectionRepository>();

// Rejestracja drzewa reguł AST — buduje composite tree z fuzzy nodes
builder.Services.AddScoped<IRuleNode>(sp =>
{
    var algebra = sp.GetRequiredService<IScoreAlgebra>();
    return DefaultRuleTreeFactory.Create(algebra);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();