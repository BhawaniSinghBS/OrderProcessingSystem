using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemApplication.UserService
{
    public interface IUserService
    {
        Task<UserDTO> AuthenticateUserAsync(string username, string password);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<bool> AddUserAsync(UserDTO user, string password);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserDTO> GetCustomerByIdAsync(int userId);
        Task<List<UserDTO>> GetAllCustomersAsync();
    }
}
