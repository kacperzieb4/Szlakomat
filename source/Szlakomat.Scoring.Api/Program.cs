using MediatR;
using Szlakomat.Scoring.Application.Mappers;
using FluentValidation;
using Szlakomat.Scoring.Application.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IEventMapper, EventMapper>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();