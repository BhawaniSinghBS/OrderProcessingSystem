using Microsoft.JSInterop;
using OrderProcessingSystem.Shared.Models.DTOs;
using System.Text.Json;

namespace OrderProcessingSystem.Client.Services
{
    public static class UserContext
    {
        private static UserDTO _user;
        public static event Action<UserDTO> OnUserChanged;

        // Property to access the current user
        public static UserDTO User
        {
            get => _user;
            private set
            {
                _user = value;
                OnUserChanged?.Invoke(value);
            }
        }

        // Method to set the user and store it in local storage
        public static async Task SetUser(IJSRuntime jsRuntime, UserDTO user)
        {
            _user = user;
            // Store user in local storage
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "authUser", JsonSerializer.Serialize(user));
            OnUserChanged?.Invoke(user);
        }

        // Method to load the user from local storage
        public static async Task LoadUserFromLocalStorage(IJSRuntime jsRuntime)
        {
            var userJson = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authUser");
            if (!string.IsNullOrEmpty(userJson))
            {
                _user = JsonSerializer.Deserialize<UserDTO>(userJson);
                OnUserChanged?.Invoke(_user);
            }
        }

        // Method to clear user data (on logout)
        public static async Task ClearUser(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authUser");
            _user = null;
            OnUserChanged?.Invoke(null);
        }
    }
}
