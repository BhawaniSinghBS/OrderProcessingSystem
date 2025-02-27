using Microsoft.Extensions.DependencyInjection;
using OrderProcessingSystemApplication.OrderService;
using OrderProcessingSystemApplication.ProductService;
using OrderProcessingSystemApplication.UserService;

namespace OrderProcessingSystemApplication
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService.UserService>();
            services.AddScoped<IOrderService, OrderService.OrderService>();
            services.AddScoped<IProductService, ProductService.ProductService>();
            return services;
        }
    }
}
