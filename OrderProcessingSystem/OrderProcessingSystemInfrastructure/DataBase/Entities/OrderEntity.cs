namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsFulfilled { get; set; } = false;

        // Navigation properties
        public UserEntity Customer { get; set; }
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();

        // Calculated Total Price
        public decimal TotalPrice => OrderProducts.Sum(op => op.TotalPrice);
    }
}
