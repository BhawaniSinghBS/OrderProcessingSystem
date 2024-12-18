using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.UserRepo;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Claims;

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
                    user = await _context.Users
                                            .Where(w => w.Id==user.Id)
                                            .Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                            .Include(u => u.UserClaims)
                                            .FirstOrDefaultAsync();
                }
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

        // Get user by ID
        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                         .Where(u => u.Id == userId && u.IsCustomer)
                         .Include(u => u.UserRoles)
                             .ThenInclude(ur => ur.Role)
                         .Include(u => u.UserClaims)
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

        // Get all users
        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            try
            {
                // var users = await _context.Set<UserEntity>().ToListAsync();
                var users = await _context.Users
                        .Include(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.UserClaims)
                        .ToListAsync();
                return users ?? new();
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


        // Add a new user
        public async Task<bool> AddUserAsync(UserEntity user, string password)
        {
            try
            {
                if (user != null)
                {
                    return await AddUserAsync(user, password, user.UserClaims, user.UserRoles);
                }
                return false;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return false;
            }
        }
        //add user with claims and roles
        public async Task<bool> AddUserAsync(UserEntity user, string password, IEnumerable<IdentityUserClaim<int>> claims, IEnumerable<UserRoleEntity> roles)
        {
            try
            {
                // Create the user
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    Log.Error("Failed to create user. Errors: {@Errors}", result.Errors);
                    return false;
                }

                // Add claims
                if (claims?.Count() > 0 && !await AddClaimsAsync(user, claims))
                {
                    Log.Error("Failed to add claims for user {UserName}.", user.UserName);
                    return false;
                }

                // Add roles
                if (roles?.Count() > 0 && !await AddRolesAsync(user, roles))
                {
                    Log.Error("Failed to add roles for user {UserName}.", user.UserName);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}------";
                Log.Error(ex, exLocationAndMessage);
                return false;
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
                var user = await _context.Users
                                        .Where(u => u.Id == userId && u.IsCustomer)
                                        .Include(u => u.UserRoles)
                                            .ThenInclude(ur => ur.Role)
                                        .Include(u => u.UserClaims)
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
                var users = await _context.Users
                      .Where(u => u.IsCustomer)
                      .Include(u => u.UserRoles)
                          .ThenInclude(ur => ur.Role)
                      .Include(u => u.UserClaims)
                      .ToListAsync();
                return users ?? new();
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return new();
            }
        }
        public async Task<bool> AddRolesAsync(UserEntity user, IEnumerable<UserRoleEntity> roles)
        {
            foreach (var role in roles)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, role.Role.Name ?? "");
                if (!roleResult.Succeeded)
                {
                    Log.Error("Failed to add role {Role} to user {UserName}. Errors: {@Errors}",
                               role, user.UserName, roleResult.Errors);
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> AddClaimsAsync(UserEntity user, IEnumerable<IdentityUserClaim<int>> claims)
        {
            foreach (var claim in claims)
            {
                var claimResult = await _userManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue));
                if (!claimResult.Succeeded)
                {
                    Log.Error("Failed to add claim {ClaimType} to user {UserName}. Errors: {@Errors}",
                               claim.ClaimType, user.UserName, claimResult.Errors);
                    return false;
                }
            }
            return true;
        }
    }
}
