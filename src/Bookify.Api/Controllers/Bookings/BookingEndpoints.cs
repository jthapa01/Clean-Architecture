using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;

namespace Bookify.Api.Controllers.Bookings;
public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("bookings/{id:guid}", GetBooking)
            .RequireAuthorization()
            .WithName(nameof(GetBooking));

        builder.MapPost("bookings", ReserveBooking)
            .RequireAuthorization();

        return builder;
    }

    public static async Task<IResult> GetBooking(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    private static async Task<IResult> ReserveBooking(ReserveBookingRequest request, ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.CreatedAtRoute(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}