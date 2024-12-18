using OrderProcessingSystemInfrastructure.DataBase.Entities;
using System.Security.Claims;

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
        Task<bool> AddRolesAsync(UserEntity user, List<string> roles);
        Task<bool> AddClaimsAsync(UserEntity user, List<Claim> claims);
        Task<bool> AddUserAsync(UserEntity user, string password, List<Claim> claims, List<string> roles);
    }
}
