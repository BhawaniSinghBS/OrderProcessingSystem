namespace OrderProcessingSystem.Shared.AppSettingKeys
{
    public class AppsettingsKeys
    {
        #region sectionNames
        public static string JWTIssuerKey => "Issuer";
        public static string JWTAudienceKey => "Audience";
        public static string JWTSecretKeyKey => "SecretKey";
        public static string AuthTokenExpiryInSeconds => "AuthTokenExpiryInSeconds";
        public static string ConnectionStringsSectionName => "ConnectionStrings";
        public static string JWTSectionName => "Jwt";
        #endregion sectionNames
        public static string AllowedHostsKey => "AllowedHosts";
        public static string BadRequestTextMessageKey => "BadRequestTextMessage";
    }
}
