namespace OrderProcessingSystem.Shared.Enums
{
    public static class EnumHelper
    {
        public static Dictionary<string, int> GetEnumValueAsKeyValue<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            var dictionary = new Dictionary<string, int>();
            foreach (TEnum name in Enum.GetValues(typeof(TEnum)))
            {
                dictionary.Add(name.ToString(), Convert.ToInt32(name));
            }
            return dictionary;
        }

        public static Dictionary<string, int> ToDictionary<TEnum>() where TEnum : Enum
        {
            var dictionary = new Dictionary<string, int>();
            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                dictionary.Add(value.ToString(), Convert.ToInt32(value));
            }
            return dictionary;
        }
    }
}
