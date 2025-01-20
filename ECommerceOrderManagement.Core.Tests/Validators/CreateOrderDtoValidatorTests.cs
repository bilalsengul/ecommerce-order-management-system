using ECommerceOrderManagement.Core.DTOs;
using ECommerceOrderManagement.Core.Validators;
using FluentAssertions;
using Xunit;

namespace ECommerceOrderManagement.Core.Tests.Validators;

public class CreateOrderDtoValidatorTests
{
    private readonly CreateOrderDtoValidator _validator;

    public CreateOrderDtoValidatorTests()
    {
        _validator = new CreateOrderDtoValidator();
    }

    [Fact]
    public void Validate_WhenOrderItemsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var createOrderDto = new CreateOrderDto
        {
            UserId = Guid.NewGuid(),
            OrderItems = new List<CreateOrderItemDto>()
        };

        // Act
        var result = _validator.Validate(createOrderDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OrderItems");
    }

    [Fact]
    public void Validate_WhenUserIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var createOrderDto = new CreateOrderDto
        {
            UserId = Guid.Empty,
            OrderItems = new List<CreateOrderItemDto>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act
        var result = _validator.Validate(createOrderDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId");
    }

    [Fact]
    public void Validate_WhenValidCreateOrderDto_ShouldNotHaveValidationError()
    {
        // Arrange
        var createOrderDto = new CreateOrderDto
        {
            UserId = Guid.NewGuid(),
            OrderItems = new List<CreateOrderItemDto>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act
        var result = _validator.Validate(createOrderDto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
} 