using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceOrderManagement.Core.Entities;

namespace ECommerceOrderManagement.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Order> GetOrderWithDetailsAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByAmountRangeAsync(decimal minAmount, decimal maxAmount);
    }
} 