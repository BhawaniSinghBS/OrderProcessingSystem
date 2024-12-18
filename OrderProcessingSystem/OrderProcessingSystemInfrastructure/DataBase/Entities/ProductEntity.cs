namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Navigation property: Many-to-Many relationship through the join entity
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();
    }
}
