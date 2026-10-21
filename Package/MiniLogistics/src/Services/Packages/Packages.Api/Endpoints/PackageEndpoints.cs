using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Packages.Api.Data;
using Packages.Api.Dtos;
using Packages.Api.Models;
namespace Packages.Api.Endpoints;

public static class PackageEndpoints
{
    public static void MapPackageEndpoints(this WebApplication app)
    {
        //metodo para crear un paquete
        app.MapPost("/packages", async (CreatePackageRequest request, PackagesDbContext db) =>
        {
            var package = new Package
            {
                Description = request.Description,
                weight = request.Weight,
                origin = request.Origin,
                destination = request.Destination
            };

            db.Packages.Add(package);

            await db.SaveChangesAsync();

            return Results.Created($"/packages/{package.Id}", package);
        });
        
        //metodo para obtener todos los paquetes
        app.MapGet("/packages", async (PackagesDbContext db) =>
        {
            var packages = await db.Packages.ToListAsync();
            return Results.Ok(packages);
        });
        //metodo para obtener un paquete por id
        app.MapGet("/packages/{id}", async (int id, PackagesDbContext db) =>
        {
            var package = await db.Packages.FindAsync(id);
            if (package == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(package);
        });
        // metodo para eliminar un paquete por id
        app.MapDelete("/packages/{id}", async (int id, PackagesDbContext db) =>
        {
            var package = await db.Packages.FindAsync(id);
            if (package == null)
            {
                return Results.NotFound();
            }
            db.Packages.Remove(package);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}

