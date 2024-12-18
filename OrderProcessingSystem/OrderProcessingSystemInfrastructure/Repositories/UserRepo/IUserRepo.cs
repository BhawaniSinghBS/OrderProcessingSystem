using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemInfrastructure.Repositories.AuthenticateUserRepo
{
    public interface IUserRepo
    {
        Task<UserEntity> AuthenticateUserAsync(string username, string password);
        Task<UserEntity> GetUserByIdAsync(int userId);
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<bool> AddUserAsync(UserEntity user, string password);
        Task<bool> UpdateUserAsync(UserEntity user);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserEntity> GetCustomerByIdAsync(int userId);
        Task<List<UserEntity>> GetAllCustomersAsync();
    }
}
