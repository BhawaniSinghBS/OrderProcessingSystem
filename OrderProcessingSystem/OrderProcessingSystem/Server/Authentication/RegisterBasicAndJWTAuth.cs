using Microsoft.AspNetCore.Authentication;

namespace OrderProcessingSystem.Server.Authentication
{
    public static class RegisterAuthentication
    {
        public static void RegisterBasicAndJWTAuthenticaton(this IServiceCollection services)
        {
            // Basic Authentication for the token endpoint
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthEnums.AuthenticationSchemes.Basic.ToString();
                options.DefaultChallengeScheme = AuthEnums.AuthenticationSchemes.Basic.ToString();
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAndJWTAuthenticationHandler>(AuthEnums.AuthenticationSchemes.Basic.ToString(), options => { })
            .AddScheme<AuthenticationSchemeOptions, BasicAndJWTAuthenticationHandler>(AuthEnums.AuthenticationSchemes.Bearer.ToString(), options => { });
            services.AddControllers();
        }
    }
}
