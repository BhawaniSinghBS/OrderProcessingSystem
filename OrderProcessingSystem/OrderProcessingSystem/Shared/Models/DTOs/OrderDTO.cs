using System.Text.Json.Serialization;

namespace OrderProcessingSystem.Shared.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsFulfilled { get; set; } = false;
        public List<OrderProductModel> OrderProducts { get; set; } = new();// product ids
    }
}