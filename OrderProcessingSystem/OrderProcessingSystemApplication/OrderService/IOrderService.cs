using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemApplication.OrderService
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int orderId);
        Task<bool> AddOrderAsync(OrderDTO order);
        Task<bool> UpdateOrderAsync(OrderDTO order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<bool> HasUnfulfilledOrdersAsync(int customerId);
    }
}
