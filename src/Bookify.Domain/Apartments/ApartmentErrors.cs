using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments;

public static class ApartmentErrors
{
    public static Error NotFound => new(
        "Apartment not found",
        "The apartment was not found.");
}