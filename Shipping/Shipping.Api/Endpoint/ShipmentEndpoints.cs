using Microsoft.EntityFrameworkCore;
using Shipping.Api.Client;
using Shipping.Api.Data;
using Shipping.Api.Models;
using Shipping.Api.DTO;

namespace Shipping.Api.Endpoints;

public static class ShipmentEndpoints
{
    public static void MapShipmentEndpoints(this WebApplication app)
    {
        app.MapPost("/shipments", async (
            Shipment shipment,
            ShippingDbContext db,
            PackagesClient packagesClient) =>
        {
            // 1. Comprobar que el paquete existe
            var packageExists =
                await packagesClient.PackageExistsAsync(
                    shipment.PackageId);
            if (!packageExists)
            {
                return Results.BadRequest(
                    "El paquete indicado no existe.");
            }
            var shipmentExists = await db.Shipments
              .AnyAsync(s => s.PackageId == shipment.PackageId);

            if (shipmentExists)
            {
                return Results.Conflict(
                    "Este paquete ya tiene un envío asociado.");
            }

            // 2. Establecer los valores iniciales
            shipment.Id = 0;
            shipment.Status = "Created";
            shipment.CreatedAt = DateTime.UtcNow;
            shipment.UpdatedAt = DateTime.UtcNow;

            // 3. Guardar el envío en la base de datos
            db.Shipments.Add(shipment);
            await db.SaveChangesAsync();

            // 4. Devolver la respuesta HTTP
            return Results.Created(
                $"/shipments/{shipment.Id}",
                shipment);
        });
        app.MapGet("/shipments", async (
            ShippingDbContext db) =>
        {
            var shipments = await db.Shipments.ToListAsync();
            return Results.Ok(shipments);
        });

        app.MapGet("/shipments/{id:int}", async (
            int id,
            ShippingDbContext db) =>
        {
            var shipment = await db.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return Results.NotFound("El envío indicado no existe.");
            }
            return Results.Ok(shipment);
        });
        app.MapDelete("/shipments/{id:int}", async (
            int id,
            ShippingDbContext db) =>
        {
            var shipment = await db.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return Results.NotFound("El envío indicado no existe.");
            }
            db.Shipments.Remove(shipment);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });


        app.MapPut("/shipments/{id:int}/status", async (
            int id,
            UpdateShipmentStatusRequest request,
            ShippingDbContext db,
            TrackingClient trackingClient) =>
        {
            var shipment = await db.Shipments.FindAsync(id);

            if (shipment is null)
            {
                return Results.NotFound(
                    "El envío indicado no existe.");
            }

            if (request.Status is not ("Created" or "InTransit" or "Delivered"))
            {
                return Results.BadRequest(
                    "El estado debe ser Created, InTransit o Delivered.");
            }

            if (string.IsNullOrWhiteSpace(request.Location))
            {
                return Results.BadRequest(
                    "La ubicación es obligatoria.");
            }

            shipment.Status = request.Status;
            shipment.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            var trackingResult = await trackingClient.CreateTrackingEventAsync(
                shipment.PackageId,
                shipment.Status,
                request.Location.Trim());

            if (!trackingResult.Success)
            {
                return Results.Problem(
                    detail: trackingResult.Message,
                    title: "Error al registrar el seguimiento",
                    statusCode: StatusCodes.Status502BadGateway);
            }

            return Results.Ok(shipment);
        });




    }
}
    
