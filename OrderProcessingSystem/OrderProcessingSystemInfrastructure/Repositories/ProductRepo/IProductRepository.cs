using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemInfrastructure.Repositories.ProductRepo
{

    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity> GetByIdAsync(int id);
        Task<bool> AddAsync(ProductEntity product);
        Task<bool> UpdateAsync(ProductEntity product);
        Task<bool> DeleteAsync(int id);
    }

}
