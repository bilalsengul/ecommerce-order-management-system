using ECommerceOrderManagement.Core.DTOs;
using ECommerceOrderManagement.Core.Validators;
using FluentAssertions;
using Xunit;

namespace ECommerceOrderManagement.Core.Tests.Validators;

public class CreateOrderItemDtoValidatorTests
{
    private readonly CreateOrderItemDtoValidator _validator;

    public CreateOrderItemDtoValidatorTests()
    {
        _validator = new CreateOrderItemDtoValidator();
    }

    [Fact]
    public void Validate_WhenProductIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var createOrderItemDto = new CreateOrderItemDto
        {
            ProductId = Guid.Empty,
            Quantity = 1
        };

        // Act
        var result = _validator.Validate(createOrderItemDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenQuantityIsLessThanOrEqualToZero_ShouldHaveValidationError(int quantity)
    {
        // Arrange
        var createOrderItemDto = new CreateOrderItemDto
        {
            ProductId = Guid.NewGuid(),
            Quantity = quantity
        };

        // Act
        var result = _validator.Validate(createOrderItemDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
    }

    [Fact]
    public void Validate_WhenValidCreateOrderItemDto_ShouldNotHaveValidationError()
    {
        // Arrange
        var createOrderItemDto = new CreateOrderItemDto
        {
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        // Act
        var result = _validator.Validate(createOrderItemDto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
} 