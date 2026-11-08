using JamunaBank.Procurement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddApiFoundation(builder.Configuration);

var app = builder.Build();
app.UseApiFoundation();
app.MapControllers();
app.Run();

public partial class Program;
