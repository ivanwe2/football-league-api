using FootballLeague.API.Endpoints;
using FootballLeague.API.Extensions;
using FootballLeague.API.Middleware;
using FootballLeague.API.Seed;
using FootballLeague.Data;
using FootballLeague.Services;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddBusinessLayer();

builder.Services.AddFeatureManagement();
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

await app.ApplyMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapTeamEndpoints();
app.MapMatchEndpoints();

app.Run();