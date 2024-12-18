using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OrderProcessingSystem.Server.Authentication;
using OrderProcessingSystem.Server.Middleware;
using OrderProcessingSystem.Server.ObjectMapper;
using OrderProcessingSystem.Shared;
using OrderProcessingSystem.Shared.AppsettingKeys;
using OrderProcessingSystemApplication;
using OrderProcessingSystemInfrastructure;
using OrderProcessingSystemInfrastructure.DataBase;
using Serilog;

namespace OrderProcessingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region appsettings 
            var configuration = builder.Configuration;
            AppsettingsHelper.UpdateAppSettings(configuration);// will be called on app start
            if (configuration is IConfigurationRoot configRoot)// using this method instead of dependency injection
            {
                ChangeToken.OnChange(() => configRoot.GetReloadToken(), () =>
                {
                    AppsettingsHelper.UpdateAppSettings(configuration);// will be called when save button is prssed in appsettings.json file
                });
            }
            #endregion appsettings 

            #region addSerilog
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)  // Read settings from appsettings.json
                        .CreateLogger();

            // Replace the default logger with Serilog
            builder.Host.UseSerilog();
            #endregion addSerilog

            if (!(GlobalSettings.AllowedHosts?.Contains("*") ?? true))
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder =>// if cliet address is static and known then to them only*/
                        builder.WithOrigins(GlobalSettings.AllowedHosts.Split(';'))
                               .SetIsOriginAllowedToAllowWildcardSubdomains()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials());
                });
            }
            else
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAnyOrigin", builder =>
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader());
                });
            }
            // Add services to the container.
            builder.Services.RegisterMapsterMapper();
            builder.Services.RegisterBasicAndJWTAuthenticaton();
            builder.Services.AddInfrastructureServices(configuration.GetConnectionString("DBConnection")); 
            builder.Services.AddApplicationServices();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            //add swager
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            if (!GlobalSettings.AllowedHosts?.Contains("*") ?? false)
            {
                app.UseCors("AllowSpecificOrigin");
            }
            else
            { 
                app.UseCors("AllowAnyOrigin");
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Add Swagger services
          
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.Run();
        } 
    }
}
