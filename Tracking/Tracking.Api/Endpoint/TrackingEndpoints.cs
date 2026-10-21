
using Microsoft.EntityFrameworkCore;
using Tracking.Api.Dto;
using Tracking.Api.Data;
using Tracking.Api.Models;

namespace Tracking.Api.Endpoints;

public static class TrackingEndpoints
{
    public static void MapTrackingEndpoints(this WebApplication app)
    {
        // Registrar un evento de seguimiento
        app.MapPost("/tracking", async (
            CreateTrackingEventRequest request,
            TrackingDbContext db) =>
        {
            if (request.PackageId <= 0)
            {
                return Results.BadRequest(
                    "El PackageId debe ser mayor que cero.");
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return Results.BadRequest(
                    "El estado es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(request.Location))
            {
                return Results.BadRequest(
                    "La ubicación es obligatoria.");
            }

            var trackingEvent = new TrackingEvent
            {
                PackageId = request.PackageId,
                Status = request.Status.Trim(),
                Location = request.Location.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            db.TrackingEvents.Add(trackingEvent);
            await db.SaveChangesAsync();

            return Results.Created(
                $"/tracking/{trackingEvent.PackageId}",
                trackingEvent);
        });

        // Consultar el historial de un paquete
        app.MapGet("/tracking/{packageId:int}", async (
            int packageId,
            TrackingDbContext db) =>
        {
            if (packageId <= 0)
            {
                return Results.BadRequest(
                    "El PackageId debe ser mayor que cero.");
            }

            var events = await db.TrackingEvents
                .Where(e => e.PackageId == packageId)
                .OrderBy(e => e.CreatedAt)
                .ThenBy(e => e.Id)
                .ToListAsync();

            return Results.Ok(events);
        });
    }
}

