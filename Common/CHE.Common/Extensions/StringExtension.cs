namespace CHE.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidString(this string stringToValidate) =>
            !string.IsNullOrEmpty(stringToValidate) && !string.IsNullOrWhiteSpace(stringToValidate);
    }
}