using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemInfrastructure.Repositories.OrderRepo
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        Task<List<OrderEntity>> GetAllOrdersAsync();

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        Task<OrderEntity> GetOrderByIdAsync(int orderId);

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">The order entity to add.</param>
        Task<bool> AddOrderAsync(OrderEntity order);

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="order">The updated order entity.</param>
        Task<bool> UpdateOrderAsync(OrderEntity order);

        /// <summary>
        /// Deletes an order from the database based on its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        Task<bool> DeleteOrderAsync(int orderId);

        /// <summary>
        /// Checks if a customer has any unfulfilled orders.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        Task<bool> HasUnfulfilledOrdersAsync(int customerId);
        /// <summary>
        /// get orders by customer (userid)
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<List<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
