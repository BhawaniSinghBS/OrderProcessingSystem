using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                services.AddDbContext<DataBaseContext>(options => 
                        //options.UseLazyLoadingProxies() 
                        //.UseSqlServer(connectionStrings));
                        options.UseSqlServer(connectionStrings));
                services.AddIdentity<UserEntity, RoleEntity>()
                        .AddEntityFrameworkStores<DataBaseContext>()
                        .AddDefaultTokenProviders();
                services.Configure<IdentityOptions>(options =>
                {
                    options.Lockout.MaxFailedAccessAttempts = 5; // Lockout after 5 failed login attempts
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout for 15 minutes
                    options.Lockout.AllowedForNewUsers = true; // Enable lockout for new users
                });
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
