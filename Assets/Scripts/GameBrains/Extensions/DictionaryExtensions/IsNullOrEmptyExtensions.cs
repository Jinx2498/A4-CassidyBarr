using System.Collections;

namespace GameBrains.Extensions.DictionaryExtensions
{
    public static class IsNullOrEmptyExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count < 1;
        }

        public static bool IsNullOrEmpty(this IDictionary dictionary)
        {
            return dictionary == null || dictionary.Count < 1;
        }

        public static bool IsNullOrEmpty(this string theString)
        {
            return System.String.IsNullOrEmpty(theString);
        }
        
        public static bool IsNullOrWhiteSpace(this string theString)
        {
            return System.String.IsNullOrWhiteSpace(theString);
        }
    }
}