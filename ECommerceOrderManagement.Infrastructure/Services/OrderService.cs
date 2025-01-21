using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ECommerceOrderManagement.Core.Entities;
using ECommerceOrderManagement.Core.Interfaces;

namespace ECommerceOrderManagement.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IWebhookService _webhookService;
        private readonly ICacheService _cacheService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IRepository<Product> productRepository,
            IWebhookService webhookService,
            ICacheService cacheService,
            IMessageQueueService messageQueueService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _webhookService = webhookService;
            _cacheService = cacheService;
            _messageQueueService = messageQueueService;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Guid userId, IEnumerable<(Guid ProductId, int Quantity)> orderItems)
        {
            try
            {
                if (!await ValidateOrderAsync(orderItems))
                {
                    throw new InvalidOperationException("Invalid order items or insufficient stock");
                }

                var order = new Order
                {
                    UserId = userId,
                    Status = "Created"
                };

                decimal totalAmount = 0;
                foreach (var item in orderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price * item.Quantity
                    };

                    order.OrderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;

                    // Update product stock
                    product.StockQuantity -= item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }

                order.TotalAmount = totalAmount;
                await _orderRepository.AddAsync(order);

                // Send webhook notification
                await _webhookService.SendOrderCreatedWebhookAsync(order);

                // Publish message to queue
                await _messageQueueService.PublishAsync("order-created", order);

                // Invalidate cache
                await _cacheService.RemoveAsync($"order:{order.Id}");
                await _cacheService.RemoveAsync($"user-orders:{userId}");

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            var cacheKey = $"order:{orderId}";
            var cachedOrder = await _cacheService.GetAsync<Order>(cacheKey);
            
            if (cachedOrder != null)
                return cachedOrder;

            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            
            if (order != null)
                await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromMinutes(30));

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId)
        {
            var cacheKey = $"user-orders:{userId}";
            var cachedOrders = await _cacheService.GetAsync<IEnumerable<Order>>(cacheKey);
            
            if (cachedOrders != null)
                return cachedOrders;

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            
            if (orders.Any())
                await _cacheService.SetAsync(cacheKey, orders, TimeSpan.FromMinutes(30));

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Order>> GetOrdersByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _orderRepository.GetOrdersByAmountRangeAsync(minAmount, maxAmount);
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                throw new InvalidOperationException("Order not found");

            if (order.Status == "Cancelled")
                throw new InvalidOperationException("Order is already cancelled");

            // Restore product stock
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                product.StockQuantity += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            order.Status = "Cancelled";
            await _orderRepository.UpdateAsync(order);

            // Send webhook notification
            await _webhookService.SendOrderCancelledWebhookAsync(order);

            // Publish message to queue
            await _messageQueueService.PublishAsync("order-cancelled", order);

            // Invalidate cache
            await _cacheService.RemoveAsync($"order:{order.Id}");
            await _cacheService.RemoveAsync($"user-orders:{order.UserId}");
        }

        public async Task<bool> ValidateOrderAsync(IEnumerable<(Guid ProductId, int Quantity)> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                    return false;
            }
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var cacheKey = "all-orders";
            var cachedOrders = await _cacheService.GetAsync<IEnumerable<Order>>(cacheKey);
            
            if (cachedOrders != null)
                return cachedOrders;

            var orders = await _orderRepository.GetAllAsync();
            
            if (orders.Any())
                await _cacheService.SetAsync(cacheKey, orders, TimeSpan.FromMinutes(30));

            return orders;
        }
    }
} 