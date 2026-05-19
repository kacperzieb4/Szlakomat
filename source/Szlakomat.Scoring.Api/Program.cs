using MediatR;
using Szlakomat.Scoring.Application.Mappers;
using FluentValidation;
using Szlakomat.Scoring.Domain.Fuzzy;
using Szlakomat.Scoring.Domain.Projections;
using Szlakomat.Scoring.Domain.Rules;
using Szlakomat.Scoring.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEventMapper, EventMapper>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

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