using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using OrderProcessingSystem.Server.Controllers;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemApplication.OrderService;
using OrderProcessingSystemApplication.UserService;

namespace OrderProcessingSystemBackEndTesting
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IOrderService> _mockOrderService;
        //private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IConfiguration _configuration;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockOrderService = new Mock<IOrderService>();
            //_mockConfiguration = new Mock<IConfiguration>();
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to current directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Add appsettings.json

            _configuration = builder.Build();
            _controller = new UserController(_mockUserService.Object, _mockOrderService.Object, _configuration);
        }

        [Fact]
        public async Task AuthenticateUser_ReturnsOk_WhenValidUser()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "customer1@example.com", Password = "password123" };
            var user = new UserDTO { Id = 1, UserName = "customer1", Email = "customer1@example.com", Roles = new List<string> { "Customer" } };

            _mockUserService.Setup(s => s.AuthenticateUserAsync(loginDto.Email, loginDto.Password))
                            .ReturnsAsync(user);

            // Act
            var result = await _controller.AuthenticateUser(loginDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDTO>(actionResult.Value);
            Assert.Equal(user.UserName, returnValue.UserName);
        }

        [Fact]
        public async Task AuthenticateUser_ReturnsBadRequest_WhenInvalidLogin()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "wrongpassword" };

            _mockUserService.Setup(s => s.AuthenticateUserAsync(loginDto.Email, loginDto.Password))
                            .ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.AuthenticateUser(loginDto);

            // Assert
            var actionResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials", actionResult.Value);
        }

        [Fact]
        public async Task AuthenticateUser_ReturnsBadRequest_WhenMissingEmailOrPassword()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "", Password = "" };

            // Act
            var result = await _controller.AuthenticateUser(loginDto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to authenticate user,Invalid parameters", actionResult.Value);
        }
    }
}
