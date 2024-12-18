using System.ComponentModel.DataAnnotations;

namespace OrderProcessingSystem.Shared.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAuthorised { get; set; } = false;
        public bool IsCustomer { get; set; } = true; // false if Admin
        public long PhoneNumber { get; set; }
        public bool IsAnyUnfulFilledOrder { get; set; } = true;//can not place another order if it is true 
    }
}