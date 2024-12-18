namespace OrderProcessingSystem.Shared.Constants
{
    public class ApiEndPoints
    {
        #region User
        public const string AuthenticateUser = "authenticate";
        public const string GetUserById = "{id}";
        public const string GetAllUsers = "all";
        public const string AddUser = "add";
        public const string UpdateUser = "update";
        public const string DeleteUser = "{id}";
        public const string GetCustomerById = "/api/customer/{id}";
        public const string GetAllCustomers = "/api/customers";
        #endregion User
        #region Order
        public const string GetAllOrders = "all";
        public const string GetOrderById = "{id}";
        //public const string AddOrder = "add";
        public const string AddOrder = "/api/orders";
        public const string UpdateOrder = "update";
        public const string DeleteOrder = "{id}";
        #endregion Order
        #region Product
        public const string GetAllProducts = "all";
        public const string GetProductById = "{id}";
        public const string UpdateProduct = "{id}";
        public const string DeleteProduct = "{id}";
        public const string AddProduct = "add";
        #endregion Product
    }
}
