using Microsoft.AspNetCore.Identity;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.UserRepo;
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
        Task<bool> AddUserAsync(UserEntity user, string password, IEnumerable<IdentityUserClaim<int>> claims, IEnumerable<UserRoleEntity> roles);
        Task<bool> AddRolesAsync(UserEntity user, IEnumerable<UserRoleEntity> roles);
        Task<bool> AddClaimsAsync(UserEntity user, IEnumerable<IdentityUserClaim<int>> claims);
    }
}
