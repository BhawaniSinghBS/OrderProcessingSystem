namespace OrderProcessingSystem.Shared.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public bool IsAuthorised { get; set; }
        public bool IsAnyUnfillFilledOrder { get; set; } = true;//can not place another order if it is true 
    }
}
