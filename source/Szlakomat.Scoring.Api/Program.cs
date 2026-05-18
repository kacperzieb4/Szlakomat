using MediatR;
using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;
using Szlakomat.Scoring.Application.Services;
using Szlakomat.Scoring.Application.Stubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

// Rejestracja zależności modułu scoringowego (Osoba 4)
builder.Services.AddScoped<IScoreAlgebra, FuzzyScoreAlgebra>();
builder.Services.AddScoped<IScoreNormalizer>(_ => new ScoreNormalizer(maxScore: 100.0));
builder.Services.AddScoped<IScoreCalculationService, ScoreCalculationService>();

// Rejestracja atrap (Stubs) - zostaną podmienione na właściwe implementacje przez Osoby 2 i 3
builder.Services.AddScoped<IProjectionRepository, StubProjectionRepository>();
builder.Services.AddScoped<IRuleNode, StubRuleNode>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();