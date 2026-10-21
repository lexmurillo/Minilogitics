using Microsoft.EntityFrameworkCore;
using Packages.Api.Data;
using Packages.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<PackagesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PackagesDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPackageEndpoints();

app.Run();
