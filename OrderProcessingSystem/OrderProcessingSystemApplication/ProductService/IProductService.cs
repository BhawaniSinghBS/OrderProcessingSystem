using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemApplication.ProductService
{
    public interface IProductService
    {
        Task<List<ProductEntity>> GetAllProductsAsync();
        Task<ProductEntity> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(ProductEntity product);
        Task<bool> UpdateProductAsync(ProductEntity product);
        Task<bool> DeleteProductAsync(int id);
    }
}
