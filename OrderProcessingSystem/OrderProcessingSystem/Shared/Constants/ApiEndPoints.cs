namespace OrderProcessingSystem.Shared.Constants
{
    public class ApiEndPoints
    {
        #region User
        public const string AuthenticateUser = "api/User/authenticate";
        public const string GetUserById = "api/User/{id}";
        public const string GetAllUsers = "api/User/all";
        public const string AddUser = "api/User/add";
        public const string UpdateUser = "api/User/update";
        public const string DeleteUser = "api/User/{id}";
        public const string GetCustomerById = "/api/customer/{id}";
        public const string GetAllCustomers = "/api/customers";
        #endregion User
        #region Order
        public const string GetAllOrders = "api/Order/all";
        public const string GetOrderById = "api/Order/{id}";
        //public const string AddOrder = "add";
        public const string AddOrder = "/api/orders";
        public const string UpdateOrder = "api/Order/update";
        public const string DeleteOrder = "api/Order/{id}";
        public const string GetOrdersByCustomerId = "api/Order/{customerId}";
        #endregion Order
        #region Product
        public const string GetAllProducts = "api/Product/all";
        public const string GetProductById = "api/Product/{id}";
        public const string UpdateProduct = "api/Product/{id}";
        public const string DeleteProduct = "api/Product/{id}";
        public const string AddProduct = "api/Product/add";
        #endregion Product
    }
}
