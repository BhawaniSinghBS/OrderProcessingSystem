using Microsoft.AspNetCore.Identity;
using OrderProcessingSystemInfrastructure.Repositories.UserRepo;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{
    public class UserEntity : IdentityUser<int> // user can be customer or admin
    {
        public bool IsCustomer { get; set; } = true; // false if Admin
        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        //Navigation properties for roles and claims
        public virtual ICollection<IdentityUserClaim<int>> UserClaims { get; set; } = new List<IdentityUserClaim<int>>();
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    }
}
