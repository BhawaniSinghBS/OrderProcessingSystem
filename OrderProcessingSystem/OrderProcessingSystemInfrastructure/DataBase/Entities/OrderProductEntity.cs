namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class OrderProductEntity
    {
        public int OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }

        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
            
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.Price * Quantity;
    }
}
