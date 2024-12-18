using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrderProcessingSystem.Client.Services;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Http;
using OrderProcessingSystem.Shared.Models.DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

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
                // Combine username and password for Basic Authentication
                var credentials = $"{LoginModel.Email}:{LoginModel.Password}";
                var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

                // Create the HTTP request
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiEndPoints.AuthenticateUser)
                {
                    Content = JsonContent.Create(LoginModel)
                };
                //requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                // Send the request
                var response = await _Http.SendAsync(requestMessage);

                // Debugging: Log response status and headers
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Headers: {response.Headers}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentType?.MediaType == "application/json")
                    {
                        // Deserialize JSON response
                        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                        var token = response.Headers.Contains(HttpHeadersKeys.TokenKey) ? response.Headers.GetValues(HttpHeadersKeys.TokenKey).FirstOrDefault() : string.Empty;

                        if (!string.IsNullOrEmpty(token))
                        {
                            await _JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                            await UserContext.SetUser(_JSRuntime, user);
                            _Navigation.NavigateTo("/orders");
                        }
                        else
                        {
                            LoginError = "Authentication token missing.";
                        }
                    }
                    else
                    {
                        // Unexpected content type
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Unexpected Response Content: " + responseContent);
                        LoginError = "Unexpected response from the server.";
                    }
                }
                else
                {
                    // Log the response for failure
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Response: {responseContent}");
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


