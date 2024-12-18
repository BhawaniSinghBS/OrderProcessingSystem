using Microsoft.Extensions.Configuration;
using OrderProcessingSystem.Shared.AppSettingKeys;

namespace OrderProcessingSystem.Shared.AppsettingKeys
{
    public class AppsettingsHelper
    {
        public static void UpdateAppSettings(IConfiguration configuration)
        {   
            string allowedHosts = configuration.GetValue<string>(AppsettingsKeys.AllowedHostsKey);
            GlobalSettings.AllowedHosts = !string.IsNullOrEmpty(allowedHosts) ? allowedHosts : GlobalSettings.AllowedHosts;
        }
    }
}
