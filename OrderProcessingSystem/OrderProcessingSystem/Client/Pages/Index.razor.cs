using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrderProcessingSystem.Client.Services;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Http;
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
                var response = await _Http.PostAsJsonAsync(ApiEndPoints.AuthenticateUser, LoginModel);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and parse it
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    var token = response.Headers.Contains(HttpHeadersKeys.TokenKey) ? response.Headers.GetValues(HttpHeadersKeys.TokenKey).FirstOrDefault() : string.Empty;

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Store token in local storage
                        await _JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);

                        // Store user info and update UserContext
                        await UserContext.SetUser(_JSRuntime, user);

                        // Redirect to home page or dashboard
                        _Navigation.NavigateTo("/orders");
                    }
                    else
                    {
                        LoginError = "Authentication token missing.";
                    }
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


