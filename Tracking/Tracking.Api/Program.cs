
using Microsoft.EntityFrameworkCore;
using Tracking.Api.Data;
using Tracking.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Conexión con SQL Server
builder.Services.AddDbContext<TrackingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TrackingDb")));

var app = builder.Build();

// Configuración HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapTrackingEndpoints();

app.Run();

