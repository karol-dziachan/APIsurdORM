using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;
using Pr0t0k07.APIsurdORM.Application.Workers;
using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ISyntaxProvider, SyntaxProvider>();
builder.Services.AddSingleton<GenerateApplication>();

var app = builder.Build();

var start = DateTime.Now;

var serviceProvider = builder.Services.BuildServiceProvider();
var generator = serviceProvider.GetRequiredService<GenerateApplication>();
await generator.Handle();

var stop = DateTime.Now;

serviceProvider.GetRequiredService<ILogger<GenerateApplication>>().LogInformation("Whole generation duration {sec} MICROseconds", ((stop - start)/TimeSpan.TicksPerMicrosecond).TotalMicroseconds);

