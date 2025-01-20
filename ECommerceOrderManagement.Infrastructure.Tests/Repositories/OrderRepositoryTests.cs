using ECommerceOrderManagement.Core.Entities;
using ECommerceOrderManagement.Infrastructure.Data;
using ECommerceOrderManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace ECommerceOrderManagement.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new OrderRepository(_context);
    }

    [Fact]
    public async Task GetOrdersByUserIdAsync_ShouldReturnUserOrders()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Username = "johndoe",
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);

        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100,
                Status = "Pending",
                UserId = userId
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-002",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 200,
                Status = "Pending",
                UserId = userId
            }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOrdersByUserIdAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(o => o.UserId == userId);
    }

    [Fact]
    public async Task GetOrdersByDateRangeAsync_ShouldReturnOrdersInRange()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow.AddDays(1);
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Username = "johndoe",
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);

        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100,
                Status = "Pending",
                UserId = userId
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-002",
                OrderDate = DateTime.UtcNow.AddDays(-2),
                TotalAmount = 200,
                Status = "Pending",
                UserId = userId
            }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOrdersByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().HaveCount(1);
        result.Should().OnlyContain(o => o.OrderDate >= startDate && o.OrderDate <= endDate);
    }

    [Fact]
    public async Task GetOrdersByAmountRangeAsync_ShouldReturnOrdersInRange()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Username = "johndoe",
            PasswordHash = "hash"
        };
        await _context.Users.AddAsync(user);

        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-001",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100,
                Status = "Pending",
                UserId = userId
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-002",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 200,
                Status = "Pending",
                UserId = userId
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderNumber = "ORD-003",
                OrderDate = DateTime.UtcNow,
                TotalAmount = 300,
                Status = "Pending",
                UserId = userId
            }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOrdersByAmountRangeAsync(150, 250);

        // Assert
        result.Should().HaveCount(1);
        result.Should().OnlyContain(o => o.TotalAmount >= 150 && o.TotalAmount <= 250);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 