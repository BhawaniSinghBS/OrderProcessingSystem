using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemApplication.OrderService;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystem.Server.Controllers
{
    //[Authorize] //currently diabled
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Get All Orders
        [HttpGet(ApiEndPoints.GetAllOrders)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText}{ex.Message}");
            }
        }

        // Get Order by ID
        [HttpGet(ApiEndPoints.GetOrderById)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null) return NotFound($"Order with ID {id} not found.");
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, $"{TextMessages.InternalServerErrorText}{ex.Message}");
            }
        }

        // Add Order
        [HttpPost(ApiEndPoints.AddOrder)]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO order)
        {
            try
            {
                bool doesThisUserHasUnfullfilledOrders = await _orderService.HasUnfulfilledOrdersAsync(order.CustomerId);
                if (doesThisUserHasUnfullfilledOrders)
                {
                    return BadRequest("Failed to add order,as you already have unfullfilled orders.");

                }
                var result = await _orderService.AddOrderAsync(order);
                if (result) return Ok("Order added successfully.");
                return BadRequest("Failed to add order.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText}{ex.Message}");
            }
        }

        // Update Order
        [HttpPut(ApiEndPoints.UpdateOrder)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTO order)
        {
            try
            {
                var result = await _orderService.UpdateOrderAsync(order);
                if (result) return Ok("Order updated successfully.");
                return BadRequest("Failed to update order.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText}{ex.Message}");
            }
        }

        // Delete Order
        [HttpDelete(ApiEndPoints.DeleteOrder)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (result) return Ok("Order deleted successfully.");
                return BadRequest("Failed to delete order.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText}{ex.Message}");
            }
        }
    }
}
