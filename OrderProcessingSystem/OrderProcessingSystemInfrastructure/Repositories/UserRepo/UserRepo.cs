using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystemInfrastructure.Repositories.AuthenticateUserRepo
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly DataBaseContext _context;

        public UserRepo(UserManager<UserEntity> userManager, DataBaseContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Authenticate user using UserManager
        public async Task<UserEntity> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    return user;
                }
                return null; // Authentication failed
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Get user by ID
        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId.ToString());
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Get all users
        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            try
            {
                // Ensure we retrieve all users of type "User" (derived from IdentityUser)
                return await _context.Set<UserEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new ();
            }
        }


        // Add a new user
        public async Task<bool> AddUserAsync(UserEntity user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Update existing user details
        public async Task<bool> UpdateUserAsync(UserEntity user)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
                if (existingUser == null)
                    return false;

                existingUser.Email = user.Email;
                existingUser.UserName = user.UserName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.IsCustomer = user.IsCustomer;

                var result = await _userManager.UpdateAsync(existingUser);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Delete a user by ID
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        // Get user by ID
        public async Task<UserEntity> GetCustomerByIdAsync(int userId)
        {
            try
            {
                var user = await _userManager.Users
                                             .Where(u => u.Id == userId && u.IsCustomer)
                                             .FirstOrDefaultAsync();
                return user ?? new();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }

        /// <summary>
        /// GetAllCustomers
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> GetAllCustomersAsync()
        {
            try
            {
                // Retrieve all users where IsCustomer is true
                var customers = await _userManager.Users
                                                  .Where(u => u.IsCustomer)
                                                  .ToListAsync();
                return customers;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new List<UserEntity>();
            }
        }

    }
}
