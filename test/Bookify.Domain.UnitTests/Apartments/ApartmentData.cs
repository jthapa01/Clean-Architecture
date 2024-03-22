using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Apartments;

public static class ApartmentData
{
    public static Apartment Create(Money price, Money? cleaningFee = null, List<Amenity>? amenities = null)
        => new Apartment(
            Guid.NewGuid(),
            new Name("Apartment 1"),
            new Description("Description 1"),
            new Address("USA", "NY", "12345", "Manhattan", "123 Main St"),
            price,
            cleaningFee ?? Money.Zero(),
            amenities ?? []);
}