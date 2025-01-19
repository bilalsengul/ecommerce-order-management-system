using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ECommerceOrderManagement.Core.DTOs;
using ECommerceOrderManagement.Core.Entities;
using ECommerceOrderManagement.Core.Interfaces;

namespace ECommerceOrderManagement.API.Controllers
{
    // Temporarily disable authentication for testing
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var orderItems = createOrderDto.OrderItems.Select(item => 
                    (item.ProductId, item.Quantity)).ToList();

                var order = await _orderService.CreateOrderAsync(createOrderDto.UserId, orderItems);
                var orderDto = _mapper.Map<OrderDto>(order);

                return Ok(ApiResponse<OrderDto>.SuccessResponse(orderDto, "Order created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<OrderDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
                return NotFound(ApiResponse<OrderDto>.ErrorResponse("Order not found"));

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(ApiResponse<OrderDto>.SuccessResponse(orderDto));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetUserOrders(Guid userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orderDtos));
        }

        [HttpGet("filter/date")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetOrdersByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orderDtos));
        }

        [HttpGet("filter/amount")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetOrdersByAmountRange(
            [FromQuery] decimal minAmount,
            [FromQuery] decimal maxAmount)
        {
            var orders = await _orderService.GetOrdersByAmountRangeAsync(minAmount, maxAmount);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orderDtos));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CancelOrder(Guid id)
        {
            try
            {
                await _orderService.CancelOrderAsync(id);
                var order = await _orderService.GetOrderAsync(id);
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(ApiResponse<OrderDto>.SuccessResponse(orderDto, "Order cancelled successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<OrderDto>.ErrorResponse(ex.Message));
            }
        }
    }
} 