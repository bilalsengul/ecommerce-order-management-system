using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceOrderManagement.Core.Entities;

namespace ECommerceOrderManagement.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Guid userId, IEnumerable<(Guid ProductId, int Quantity)> orderItems);
        Task<Order> GetOrderAsync(Guid orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Order>> GetOrdersByAmountRangeAsync(decimal minAmount, decimal maxAmount);
        Task CancelOrderAsync(Guid orderId);
        Task<bool> ValidateOrderAsync(IEnumerable<(Guid ProductId, int Quantity)> orderItems);
    }
} 