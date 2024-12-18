using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Server.Authentication;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Http;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemApplication.OrderService;
using OrderProcessingSystemApplication.UserService;
using Serilog;
using System.Reflection;
using System.Security.Claims;

namespace OrderProcessingSystem.Server.Controllers
{
    //[Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IOrderService orderService, IConfiguration configuration)
        {
            _userService = userService;
            _orderService = orderService;
            _configuration = configuration;
        }

        // Authenticate User
        [HttpPost(ApiEndPoints.AuthenticateUser)]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginDTO login)
        {
            try
            {
                UserDTO user = new UserDTO();
                if (User.Identities.Any(i => i.IsAuthenticated))
                {
                    user = await _userService.AuthenticateUserAsync(login.Email, login.Password);
                    return Ok(user);
                }
                if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                {
                    return BadRequest("Failed to authenticate user,Invalid parameters");
                }
                user = await _userService.AuthenticateUserAsync(login.Email, login.Password);

                if (user == null) return Unauthorized("Invalid credentials");

                // Return the JWT token
                List<Claim> claims = AuthenticationHelper.GenerateClaims(
                    userId: user.Id.ToString(),
                    username: user.UserName,
                    roles: user.Roles,
                    permissions: user.Claims?.ToDictionary(x => x.Split(":")[0], x => bool.Parse(x.Split(":")[1])) ?? new(),
                    otherClaims: new Dictionary<string, string>() { { AllClaimTypes.Email, user.Email } }
                    );

                // Create JWT token and add to claims to add to identity so that can be aceesed in cntoller User
                var jwtToken = AuthenticationHelper.GenerateJwtToken(claims, _configuration);
                if (!AuthenticateJWTTokenAndCreateUser(jwtToken, out AuthenticationTicket authenticationTicket, isAddTokenToResponse: true))
                {
                    return Unauthorized("Invalid credentials");
                }
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
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null) return NotFound($"User with ID {id} not found.");
                return Ok(user);
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
                var users = await _userService.GetAllUsersAsync();

                return Ok(users);
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
        [NonAction]
        private bool AuthenticateJWTTokenAndCreateUser(string jwtToken, out AuthenticationTicket authenticationTicket, bool isAddTokenToResponse)
        {
            authenticationTicket = null;
            if (!string.IsNullOrEmpty(jwtToken) && AuthenticationHelper.GetClaimsPrincipalFromValidJwt(jwtToken, _configuration, out ClaimsPrincipal claimsPrincipal))
            {
                ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
                identity.AddClaim(new Claim(AllClaimTypes.TokenKey, jwtToken));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                authenticationTicket = new AuthenticationTicket(principal, AuthEnums.AuthenticationSchemes.Basic.ToString());
                if (isAddTokenToResponse)
                {
                    if (Response.Headers.ContainsKey(HttpHeadersKeys.TokenKey))
                    {
                        // Replace the existing value
                        Response.Headers[HttpHeadersKeys.TokenKey] = jwtToken;
                    }
                    else
                    {
                        // Add the header if it does not exist
                        Response.Headers.Add(HttpHeadersKeys.TokenKey, jwtToken);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
