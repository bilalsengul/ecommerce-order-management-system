using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ECommerceOrderManagement.API.Controllers;
using ECommerceOrderManagement.Core.DTOs;
using ECommerceOrderManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Xunit;

namespace ECommerceOrderManagement.API.Tests.Controllers;

public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OrdersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ShouldReturnCreated()
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
        var response = await _client.PostAsJsonAsync("/api/orders", createOrderDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var orderDto = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
        orderDto.Should().NotBeNull();
        orderDto!.Success.Should().BeTrue();
        orderDto.Data.Should().NotBeNull();
        orderDto.Data!.OrderItems.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrders()
    {
        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderDto>>>();
        orders.Should().NotBeNull();
        orders!.Success.Should().BeTrue();
        orders.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrdersByDateRange_WithValidDates_ShouldReturnOrders()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-1).ToString("O");
        var endDate = DateTime.UtcNow.AddDays(1).ToString("O");

        // Act
        var response = await _client.GetAsync($"/api/orders/by-date-range?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderDto>>>();
        orders.Should().NotBeNull();
        orders!.Success.Should().BeTrue();
        orders.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrdersByAmountRange_WithValidAmounts_ShouldReturnOrders()
    {
        // Act
        var response = await _client.GetAsync("/api/orders/by-amount-range?minAmount=0&maxAmount=1000");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderDto>>>();
        orders.Should().NotBeNull();
        orders!.Success.Should().BeTrue();
        orders.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task CancelOrder_WithValidOrderId_ShouldReturnSuccess()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/cancel", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }
} 