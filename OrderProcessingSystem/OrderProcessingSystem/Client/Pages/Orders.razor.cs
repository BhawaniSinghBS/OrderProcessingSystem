using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrderProcessingSystem.Client.Services;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystem.Shared.Models;
using OrderProcessingSystem.Shared.Models.DTOs;
using System.Net.Http.Json;

namespace OrderProcessingSystem.Client.Pages
{
    public partial class Orders
    {
        [Inject] public HttpClient _Http { get; set; }
        [Inject] public IJSRuntime _JSRuntime { get; set; }
        public List<OrderDTO> orders = new();
        public OrderDTO currentOrder = new();
        public string errorMessage;
        public bool isEditing;


        protected override async Task OnInitializedAsync()
        {
            await LoadOrders();
            await UserContext.LoadUserFromLocalStorage(_JSRuntime);
        }

        private async Task LoadOrders()
        {
            try
            {
                if (UserContext.User.Id>0)
                {
                    string customerId = UserContext.User.Id.ToString();
                    string url = ApiEndPoints.GetOrdersByCustomerId.Replace("{customerId}", customerId);
                    orders = await _Http.GetFromJsonAsync<List<OrderDTO>>(url) ?? new();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load orders: {ex.Message}";
            }
        }

        private void AddProduct()
        {
            currentOrder.OrderProducts.Add(new OrderProductModel());
        }

        private void EditOrder(OrderDTO order)
        {
            currentOrder = new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                IsFulfilled = order.IsFulfilled,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductModel
                {
                    OrderId = op.OrderId,
                    ProductId = op.ProductId,
                    Price = op.Price,
                    Quantity = op.Quantity
                }).ToList()
            };
            isEditing = true;
        }

        private async Task SaveOrder()
        {
            try
            {
                HttpResponseMessage response;
                if (isEditing)
                {
                    response = await _Http.PutAsJsonAsync("api/orders", currentOrder);
                }
                else
                {
                    response = await _Http.PostAsJsonAsync("api/orders", currentOrder);
                }

                if (response.IsSuccessStatusCode)
                {
                    await LoadOrders();
                    currentOrder = new OrderDTO();
                    isEditing = false;
                }
                else
                {
                    errorMessage = $"Failed to save order: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to save order: {ex.Message}";
            }
        }

        private async Task DeleteOrder(int id)
        {
            try
            {
                var response = await _Http.DeleteAsync($"api/orders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadOrders();
                }
                else
                {
                    errorMessage = $"Failed to delete order: {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to delete order: {ex.Message}";
            }
        }
    }
}
