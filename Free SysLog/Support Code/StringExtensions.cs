using System;

namespace Free_SysLog
{

    static class StringExtensions
    {
        /// <summary>This function uses an IndexOf call to do a case-insensitive search. This function operates a lot like Contains().</summary>
    /// <param name="needle">The String containing what you want to search for.</param>
    /// <return>Returns a Boolean value.</return>
        public static bool CaseInsensitiveContains(this string haystack, string needle)
        {
            if (string.IsNullOrWhiteSpace(haystack) | string.IsNullOrWhiteSpace(needle))
                return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) != -1;
        }

        /// <summary>Works similar to the original String Replacement function but with a potential case-insensitive match capability.</summary>
    /// <param name="source">The source String.</param>
    /// <param name="strReplace">The String to be replaced.</param>
    /// <param name="strReplaceWith">The String that will replace all occurrences of <paramref name="strReplaceWith"/>. 
    /// If value Is equal to <c>null</c>, than all occurrences of <paramref name="strReplace"/> will be removed from the <paramref name="source"/>.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <returns>A string that Is equivalent to the current string except that all instances of <paramref name="strReplace"/> are replaced with <paramref name="strReplaceWith"/>. 
    /// If <paramref name="strReplace"/> Is Not found in the current instance, the method returns the current instance unchanged.</returns>
        public static string Replace(this string source, string strReplace, string strReplaceWith, StringComparison comparisonType)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));
            if (source.Length == 0)
                return source;
            if (string.IsNullOrWhiteSpace(strReplace))
                throw new ArgumentNullException(nameof(strReplace));
            if (strReplace.Length == 0)
                throw new ArgumentException("String cannot be of zero length.");

            var resultStringBuilder = new System.Text.StringBuilder(source.Length);
            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(strReplaceWith);

            const int valueNotFound = -1;
            var foundAt = default(int);
            int startSearchFromIndex = 0;

            while (InlineAssignHelper(ref foundAt, source.IndexOf(strReplace, startSearchFromIndex, comparisonType)) != valueNotFound)
            {
                int charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = charsUntilReplacment == 0;

                if (!isNothingToAppend)
                    resultStringBuilder.Append(source, startSearchFromIndex, charsUntilReplacment);
                if (!isReplacementNullOrEmpty)
                    resultStringBuilder.Append(strReplaceWith);

                startSearchFromIndex = foundAt + strReplace.Length;
                if (startSearchFromIndex == source.Length)
                    return resultStringBuilder.ToString();
            }

            int charsUntilStringEnd = source.Length - startSearchFromIndex;
            resultStringBuilder.Append(source, startSearchFromIndex, charsUntilStringEnd);

            return resultStringBuilder.ToString();
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
    }
}