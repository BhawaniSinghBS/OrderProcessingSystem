using System.ComponentModel.DataAnnotations.Schema;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsFulfilled { get; set; } = false;

        // Navigation properties
        public virtual UserEntity Customer { get; set; }
        public virtual ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();

        [NotMapped]
        public decimal TotalPrice => OrderProducts.Sum(op => op.TotalPrice);
    }
}
