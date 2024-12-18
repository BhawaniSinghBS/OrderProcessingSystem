using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.OrderRepo;
using Serilog;
using System.Reflection;
using Mapster;

namespace OrderProcessingSystemApplication.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            try
            {
                List<OrderEntity> orderEntities =  await _orderRepository.GetAllOrdersAsync();
                List<OrderDTO>  orders = orderEntities.Adapt<List<OrderDTO>>();
                return orders??new();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new (); 
            }
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            try
            {
                OrderEntity orderEntity = await _orderRepository.GetOrderByIdAsync(orderId);
                OrderDTO order = orderEntity.Adapt<OrderDTO>();
                return order ?? new();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new ();
            }
        }

        public async Task<bool> AddOrderAsync(OrderDTO orderReceived)
        {
            try
            {
                if (orderReceived == null || !orderReceived.OrderProducts.Any()) // Fixed property name
                {
                    Log.Warning("Invalid order data provided for addition.");
                    return false;
                }
                OrderEntity orderEntity = orderReceived.Adapt<OrderEntity>();
                return await _orderRepository.AddOrderAsync(orderEntity);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(OrderDTO orderToUpdate)
        {
            try
            {
                if (orderToUpdate == null || orderToUpdate.Id <= 0)
                {
                    Log.Warning("Invalid order data provided for update.");
                    return false;
                }
                OrderEntity orderEntity = orderToUpdate.Adapt<OrderEntity>();
                return await _orderRepository.UpdateOrderAsync(orderEntity);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    Log.Warning($"Order with ID: {orderId} not found for deletion.");
                    return false;
                }

                return await _orderRepository.DeleteOrderAsync(orderId);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }

        public async Task<bool> HasUnfulfilledOrdersAsync(int customerId)
        {
            try
            {
                return await _orderRepository.HasUnfulfilledOrdersAsync(customerId);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }
        public async Task<List<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                List<OrderEntity> orderEntities = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
                List<OrderDTO> customerOrders = orderEntities.Adapt<List<OrderDTO>>();
                return customerOrders;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new ();
            }
        } 
    }
}




