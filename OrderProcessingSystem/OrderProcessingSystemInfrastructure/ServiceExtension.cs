using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessingSystemApplication.OrderService;
using OrderProcessingSystemInfrastructure.DataBase;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.AuthenticateUserRepo;
using OrderProcessingSystemInfrastructure.Repositories.OrderRepo;
using OrderProcessingSystemInfrastructure.Repositories.ProductRepo;

namespace OrderProcessingSystemInfrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionStrings)
        {
            try
            { 
                services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connectionStrings));
                services.AddIdentity<UserEntity, IdentityRole<int>>()
                        .AddEntityFrameworkStores<DataBaseContext>()
                        .AddDefaultTokenProviders();
                services.AddScoped<IUserRepo, UserRepo>();
                services.AddScoped<IOrderRepository, OrderRepository>();
                services.AddScoped<IProductRepository, ProductRepository>();

                return services;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
