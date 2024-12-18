using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessingSystem.Shared.Models.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
