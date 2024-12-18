using Microsoft.AspNetCore.Identity;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class UserRoleEntity : IdentityUserRole<int>
    {
        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
