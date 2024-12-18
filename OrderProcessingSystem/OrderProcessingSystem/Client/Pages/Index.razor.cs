using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Models.DTOs;
using System.Net.Http.Json;

namespace OrderProcessingSystem.Client.Pages
{
    public partial class Index
    {
        [Inject] public HttpClient _Http { get; set; }
        [Inject] public NavigationManager _Navigation { get; set; }
        [Inject] public IJSRuntime _JSRuntime { get; set; }

        private LoginDTO LoginModel = new LoginDTO();
        private string LoginError;
        private string PasswordFieldType = "password";
        private string PasswordButtonText = "Show";

        private void TogglePasswordVisibility()
        {
            if (PasswordFieldType == "password")
            {
                PasswordFieldType = "text";
                PasswordButtonText = "Hide";
            }
            else
            {
                PasswordFieldType = "password";
                PasswordButtonText = "Show";
            }
        }

        private async Task LoginUser()
        {
            try
            {
                // Simulate an HTTP login call. Replace with real logic.  
                bool loginSuccessful = LoginModel.Email == "test@example.com" && LoginModel.Password == "password";

                if (loginSuccessful)
                {
                    LoginError = "";
                    Console.WriteLine("Login Successful");
                }
                else
                {
                    LoginError = "Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                LoginError = "An error occurred: " + ex.Message;
            }
        }
    }
}

