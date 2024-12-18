using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.OrderRepo;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystemApplication.OrderService
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataBaseContext _dbContext;

        public OrderRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OrderEntity>> GetAllOrdersAsync()
        {
            try
            {
                return await _dbContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        public async Task<OrderEntity> GetOrderByIdAsync(int orderId)
        {
            try
            {
                return await _dbContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId) ?? new();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return null;
            }
        }

        public async Task<bool> AddOrderAsync(OrderEntity order)
        {
            try
            {
                _dbContext.Orders.Add(order);
                return await _dbContext.SaveChangesAsync() > 0;
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

        public async Task<bool> UpdateOrderAsync(OrderEntity updatedOrder)
        {
            try
            {
                var existingOrder = await _dbContext.Orders
                    .Include(o => o.OrderProducts)
                    .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);

                if (existingOrder == null)
                    return false;

                // Update basic fields
                existingOrder.IsFulfilled = updatedOrder.IsFulfilled;
                existingOrder.CustomerId = updatedOrder.CustomerId;
                existingOrder.OrderProducts = updatedOrder.OrderProducts;

                // Update OrderProducts
                _dbContext.OrderProducts.RemoveRange(existingOrder.OrderProducts);

                _dbContext.Orders.Update(existingOrder);
                return await _dbContext.SaveChangesAsync() > 0;
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
                var order = await GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    Log.Warning($"Order with ID {orderId} not found.");
                    return false;
                }

                _dbContext.Orders.Remove(order);
                return await _dbContext.SaveChangesAsync() > 0;
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
                return await _dbContext.Orders
                    .AnyAsync(o => o.CustomerId == customerId && !o.IsFulfilled);
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

        public async Task<List<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new List<OrderEntity>();
            }
        }

    }
}