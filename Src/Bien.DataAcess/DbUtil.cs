using System.Text.RegularExpressions;

namespace Bien.DataAcess
{
    public static class DbUtil
    {
        private static readonly Regex ColumnSafeRegex = new Regex(@"[^a-zA-Z0-9_]", RegexOptions.Compiled);

        public static string CleanseFieldName(string input)
        {
            return ColumnSafeRegex.Replace(input, string.Empty);
        }
    }
}