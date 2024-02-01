using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;

namespace Bookify.Application.Reviews.addReview;

public sealed class AddReviewCommandHandler(
    IBookingRepository bookingRepository,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddReviewCommand>
{
    public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        
        if(booking is null)
            return Result.Failure(BookingErrors.NotFound);
        
        var ratingResult = Rating.Create(request.Rating);
        
        if(ratingResult.IsFailure)
            return Result.Failure(ratingResult.Error);
        
        var reviewResult = Review.Create(
            booking, 
            ratingResult.Value, 
            new Comment(request.Comment), 
            dateTimeProvider.UtcNow);
        
        if(reviewResult.IsFailure)
            return Result.Failure(reviewResult.Error);
        
        reviewRepository.Add(reviewResult.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}