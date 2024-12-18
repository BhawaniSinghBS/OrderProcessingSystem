using Microsoft.AspNetCore.Identity;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class RoleEntity : IdentityRole<int>
    {
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    }
}