using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Reviews.addReview;

public record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;