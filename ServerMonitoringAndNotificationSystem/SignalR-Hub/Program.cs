using SignalR_Hub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<AlertHub>("/signalrhub");

app.Run();
