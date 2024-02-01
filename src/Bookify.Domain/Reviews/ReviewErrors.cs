using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotEligible = new(
        "Review.NotEligible",
        "The booking is not eligible for a review due to the incomplete status");
}