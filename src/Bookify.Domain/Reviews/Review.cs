using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews.Events;

namespace Bookify.Domain.Reviews;

public sealed class Review : Entity
{
    public Guid ApartmentId { get; private set; }
    public Guid BookingId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Rating Rating { get; private set; }
    
    public Comment Comment { get; private set; }
    
    public DateTime CreatedOnUtc { get; private set; }

    private Review(
        Guid id,
        Guid apartmentId,
        Guid bookingId,
        Guid userId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc) : base(id)
    {
        ApartmentId = apartmentId;
        BookingId = bookingId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedOnUtc = createdOnUtc;
    }
    
    private Review()
    {
    }
    
    public static Result<Review> Create(
        Booking booking,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
    {
        if(booking.Status != BookingStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible);
        }
        
        var review = new Review(
            id: Guid.NewGuid(),
            apartmentId: booking.ApartmentId,
            bookingId: booking.Id,
            userId: booking.UserId,
            rating: rating,
            comment: comment,
            createdOnUtc: createdOnUtc);
        
        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));
        return review;
    }
}