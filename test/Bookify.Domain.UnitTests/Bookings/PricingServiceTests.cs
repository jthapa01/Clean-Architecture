using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_ForPeriod_WhenNoAmenities_ReturnsCorrectPrice()
    {
        // Arrange
        var price = new Money(10.0m, Shared.Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024,03,01), new DateOnly(2024,03,15));
        var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        
        // Act
        var result = pricingService.CalculatePrice(apartment, period);
        
        // Assert
        result.TotalPrice.Should().Be(expectedTotalPrice);
    }
    
    [Fact]
    public void CalculatePrice_Should_ReturnsCorrectTotalPrice_WithCleaningFee()
    {
        // Arrange
        var price = new Money(10.0m, Shared.Currency.Usd);
        var cleaningFee = new Money(1.0m, Shared.Currency.Usd);
        var period = DateRange.Create(new DateOnly(2024,03,01), new DateOnly(2024,03,15));
        var expectedTotalPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
        expectedTotalPrice += cleaningFee;
        
        var apartment = ApartmentData.Create(price, cleaningFee);
        var pricingService = new PricingService();
        
        // Act
        var result = pricingService.CalculatePrice(apartment, period);
        
        // Assert
        result.TotalPrice.Should().Be(expectedTotalPrice);
    }
    
    [Fact]
    public void CalculatePrice_Should_ReturnsCorrectTotalPrice_WithAmenitiesIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Shared.Currency.Usd);
        List<Amenity> amenities = [Amenity.Gym, Amenity.Parking];
        var parkingFeePerDay = new Money(0.15m, Shared.Currency.Usd);
        var gymFeePerDay = new Money(0, Shared.Currency.Usd);
        var totalAmenitiesFeePerDay = parkingFeePerDay + gymFeePerDay;
        var period = DateRange.Create(new DateOnly(2024,03,01), new DateOnly(2024,03,15));
        var pricePerPeriod = new Money(price.Amount * period.LengthInDays, price.Currency);
        
        var totalAmenitiesFeeForPeriod = new Money(pricePerPeriod.Amount * totalAmenitiesFeePerDay.Amount, Currency.Usd);
        
        var expectedTotalPrice = pricePerPeriod + totalAmenitiesFeeForPeriod;
        
        var apartment = ApartmentData.Create(price, amenities:amenities);
        var pricingService = new PricingService();
        
        // Act
        var result = pricingService.CalculatePrice(apartment, period);
        
        // Assert
        result.TotalPrice.Should().Be(expectedTotalPrice);
    }
}