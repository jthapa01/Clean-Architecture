using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error NotFound = new(
        "Booking.NotFound", 
        "The booking with the specified identifier was not found");
    
    public static readonly Error Overlap = new(
        "Booking.Overlap", 
        "The booking overlaps with another booking for the same apartment");
    
    public static readonly Error NotReserved = new(
        "Booking.NotReserved", 
        "The booking is not in the reserved state");
    
    public static readonly Error NotConfirmed = new(
        "Booking.NotConfirmed", 
        "The booking is not in the confirmed state");
    
    public static readonly Error AlreadyStarted = new(
        "Booking.AlreadyStarted", 
        "The booking has already started");
}