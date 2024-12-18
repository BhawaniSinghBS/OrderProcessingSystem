using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{ 
    public class UserEntity : IdentityUser<int> // user can be customer or admin
    {
        public bool IsCustomer { get; set; } = true; // false if Admin
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        // Navigation properties for roles and claims
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
