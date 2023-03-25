namespace PDFimg.Helpers
{
    public static class StringExtension
    {
        public static string CutString(this string inputString, int maxChar)
        {
            return inputString.Length > maxChar ? $"{inputString.Substring(0, maxChar)}..." : inputString;
        }
    }
}
