using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemApplication.OrderService;
using OrderProcessingSystemApplication.UserService;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystem.Server.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public UserController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        // Authenticate User
        [HttpPost(ApiEndPoints.AuthenticateUser)]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginDTO login)
        {
            try
            {
                if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                {
                    return BadRequest("Failed to authenticate user,Invalid parameters");
                }
                UserDTO user = await _userService.AuthenticateUserAsync(login.Username, login.Password);
                user.IsAnyUnfulFilledOrder = await _orderService.HasUnfulfilledOrdersAsync(user.Id);
                if (user == null) return Unauthorized("Invalid credentials");
                return Ok(user);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while authenticating.");
            }
        }

        // Get User by ID
        [HttpGet(ApiEndPoints.GetUserById)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (result == null) return NotFound($"User with ID {id} not found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while fetching the user.");
            }
        }

        // Get All Users
        [HttpGet(ApiEndPoints.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while fetching all users.");
            }
        }

        // Add User
        [HttpPost(ApiEndPoints.AddUser)]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.User.UserName) || string.IsNullOrWhiteSpace(model.User.Email))
                {
                    return BadRequest("Failed to add user,Invalid parameters");
                }
                var result = await _userService.AddUserAsync(model.User, model.Password);
                if (result) return Ok("User added successfully.");
                return BadRequest("Failed to add user.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while adding the user.");
            }
        }

        // Update User
        [HttpPut(ApiEndPoints.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user)
        {
            try
            {
                if (user == null || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Failed to update user,Invalid parameters");
                }
                var result = await _userService.UpdateUserAsync(user);
                if (result) return Ok("User updated successfully.");
                return BadRequest("Failed to update user.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while updating the user.");
            }
        }

        // Delete User
        [HttpDelete(ApiEndPoints.DeleteUser)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result) return Ok("User deleted successfully.");
                return BadRequest("Failed to delete user.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while deleting the user.");
            }
        }

        // Get Customer by ID
        [HttpGet(ApiEndPoints.GetCustomerById)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var result = await _userService.GetCustomerByIdAsync(id);
                if (result == null) return NotFound($"Customer with ID {id} not found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while fetching the customer.");
            }
        }

        // Get All Customers
        [HttpGet(ApiEndPoints.GetAllCustomers)]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var result = await _userService.GetAllCustomersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? TextMessages.UnknownClassText;
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? TextMessages.UnknownMethodText;
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName}----{ex.Message}----";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, $"{TextMessages.InternalServerErrorText} while fetching all customers.");
            }
        }
    }
}
