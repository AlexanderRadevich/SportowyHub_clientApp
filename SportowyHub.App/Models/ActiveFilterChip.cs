namespace SportowyHub.Models;

public record ActiveFilterChip(string Key, string Label)
{
    public static class Keys
    {
        public const string Sport = "sport";
        public const string Category = "category";
        public const string Location = "location";
        public const string PriceMin = "priceMin";
        public const string PriceMax = "priceMax";
        public const string Condition = "condition";
        public const string Sort = "sort";
    }
}
