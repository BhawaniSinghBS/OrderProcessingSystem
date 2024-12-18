using Mapster;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.AuthenticateUserRepo;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystemApplication.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepository)
        {
            _userRepo = userRepository;
        }

        // Authenticate user using UserManager
        public async Task<UserDTO> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                UserEntity userDBModel = await _userRepo.AuthenticateUserAsync(email, password);
                UserDTO usreModel = userDBModel.Adapt<UserDTO>();
                return usreModel;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Get user by ID
        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            try
            {
                UserEntity userDBModel = await _userRepo.GetUserByIdAsync(userId);
                UserDTO usreModel = userDBModel.Adapt<UserDTO>();
                return usreModel;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Get all users
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                List<UserEntity> userDBModels = await _userRepo.GetAllUsersAsync();
                List<UserDTO> users = userDBModels.Adapt<List<UserDTO>>();
                return users;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Add a new user
        public async Task<bool> AddUserAsync(UserDTO userModel, string password)
        {
            try
            {
                UserEntity user = userModel.Adapt<UserEntity>();
                bool isAdded = await _userRepo.AddUserAsync( user, password);
                return isAdded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Update existing user details
        public async Task<bool> UpdateUserAsync(UserDTO userModel)
        {
            try
            {
                UserEntity user = userModel.Adapt<UserEntity>();
                bool isAdded = await _userRepo.UpdateUserAsync(user);
                return isAdded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Delete a user by ID
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                bool isAdded = await _userRepo.DeleteUserAsync(userId);
                return isAdded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        public async Task<UserDTO> GetCustomerByIdAsync(int userId)
        {
            try
            {
                UserEntity customerDBModel = await _userRepo.GetCustomerByIdAsync(userId);
                UserDTO customerModel = customerDBModel.Adapt<UserDTO>();
                return customerModel;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        public async Task<List<UserDTO>> GetAllCustomersAsync()
        {
            try
            {
                List<UserEntity> customerDBModels = await _userRepo.GetAllCustomersAsync();
                List<UserDTO> customers = customerDBModels.Adapt<List<UserDTO>>();
                return customers;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }
       
    }
}
