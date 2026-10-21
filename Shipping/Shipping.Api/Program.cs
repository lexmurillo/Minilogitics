using Microsoft.EntityFrameworkCore;
using Shipping.Api.Data;
using Shipping.Api.Client;
using Shipping.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShippingDb")));
builder.Services.AddHttpClient<PackagesClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5073/");
});
builder.Services.AddHttpClient<TrackingClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5265/");
});
var app = builder.Build();
app.MapShipmentEndpoints();

// Configurar endpoints
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();