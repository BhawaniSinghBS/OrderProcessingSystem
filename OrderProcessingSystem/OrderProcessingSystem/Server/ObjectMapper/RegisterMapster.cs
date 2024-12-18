using Mapster;
using MapsterMapper;
using OrderProcessingSystem.Server.ObjectMapper.ObjectMappingConfigurations;

namespace OrderProcessingSystem.Server.ObjectMapper
{
    public static class RegisterMapster
    {
        public static IServiceCollection RegisterMapsterMapper(this IServiceCollection services)
        {

            try
            {
                var config = TypeAdapterConfig.GlobalSettings;
                config.Scan(typeof(Program).Assembly); // Scans the assembly for mappings
                // Register Mapster and IMapper
                services.AddSingleton(config);
                Config.RegisterMappings();
                services.AddScoped<IMapper, ServiceMapper>();
            }
            catch (Exception ex)
            {
                //log
            }

            return services;
        }
    }
}
