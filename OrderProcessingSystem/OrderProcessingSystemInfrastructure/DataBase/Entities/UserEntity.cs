using Microsoft.AspNetCore.Identity;

namespace OrderProcessingSystemInfrastructure.DataBase.Entities
{ 
    public class UserEntity : IdentityUser<int> // user can be customer or admin
    {
        public bool IsCustomer { get; set; } = true; // false if Admin
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}
