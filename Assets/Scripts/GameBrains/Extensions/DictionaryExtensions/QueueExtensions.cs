using System.Collections.Generic;
using System.Text;

namespace GameBrains.Extensions.CollectionsExtensions
{
    public static class QueueExtensions
    {
        public static string ToNumberedItemsString<T>(this Queue<T> queue)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (T item in queue)
            {
                sb.AppendLine(i++ + ": " + item);
            }

            return sb.ToString();
        }
    }
}