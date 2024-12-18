using Microsoft.AspNetCore.Identity;
using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystemInfrastructure.Repositories.UserRepo
{
    public class UserRoleEntity : IdentityUserRole<int>
    {
        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
