namespace OrderProcessingSystem.Server.Authentication
{
    public class AuthEnums
    {
        
        public enum AuthenticationSchemes
        {
            Unknown = 0,
            Basic = 10,
            Bearer = 20,
        }
        public enum UserRole
        {
            Unknown = 0,
            Customer = 10,
            Admin = 20,
        }

        public enum UserPermissions
        {
            Unknown = 0,
            IsAllowedToAccesServie = 10,
        }
    }
}
