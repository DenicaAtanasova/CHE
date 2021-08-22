namespace CHE.Common.Extensions
{
    using System.Globalization;

    public static class StringExtension
    {
        public static bool IsValidString(this string stringToValidate) =>
            !string.IsNullOrEmpty(stringToValidate) && 
            !string.IsNullOrWhiteSpace(stringToValidate);

        public static string ToTitleCase(this string text) =>
            new CultureInfo("en-US",false).TextInfo.ToTitleCase(text);
    }
}