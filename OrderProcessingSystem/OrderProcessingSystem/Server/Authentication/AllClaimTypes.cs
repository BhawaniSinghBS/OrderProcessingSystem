using System.Security.Claims;

namespace OrderProcessingSystem.Server.Authentication
{
    public class AllClaimTypes
    {
        public static string Name => ClaimTypes.Name;
        public static string UserId => ClaimTypes.NameIdentifier;
        public static string Role => ClaimTypes.Role;
        public static string Email => ClaimTypes.Email;
        public static string JwtExpirationTime => "exp";
        public static string JwtIssuer => "iss";
        public static string JwtAudiance => "aud";
        public static string UserPermissions => "UserPermissions";
        public static string TokenKey => "Token";
    }
}
