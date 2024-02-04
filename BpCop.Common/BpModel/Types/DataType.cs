namespace BpCop.Common.BpModel.Types
{
    public static class DataType
    {
        public const string Password = "password";
        public const string Collection = "collection";
        public const string Text = "text";
        public const string DateTime = "datetime";
        public const string Time = "time";
        public const string Date = "date";
        public const string TimeSpan = "timespan";
        public const string Number = "number";
        public const string Flag = "flag";
        public const string Image = "image";
        public const string Binary = "binary";
        public const string Runtime = "runtime"; // Not a Blue Prism type, needed to mark referenced fields of an untyped collection
    }
}
