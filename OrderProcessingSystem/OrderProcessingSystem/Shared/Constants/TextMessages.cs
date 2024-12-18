using Mapster.Models;

namespace OrderProcessingSystem.Shared.Constants
{
    public static class TextMessages 
    {
        public static string CanNotPlaceOrderUnfulfilledOrder => "Cannot place a new order while previous orders are unfulfilled.";
        public static string UnknownClassText  => "Unknown Class";
        public static string UnknownMethodText => "Unknown Method";
        public static string FunctionNameText => "Function Name";
        public static string ClassNameText => "Class Name ";
        public static string InternalServerErrorText  => $"{TextMessages.InternalServerErrorText}";
    }
}
