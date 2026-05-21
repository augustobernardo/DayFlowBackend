using DayFlow.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceExtensions(builder.Configuration);

var app = builder.Build();

app.UseApplicationExtensions();

app.Run();
