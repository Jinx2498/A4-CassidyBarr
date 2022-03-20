using System.Collections.Generic;
using System.Text;

namespace GameBrains.Extensions.Lists
{
	// Extensions to the generic list class to give it an interface like a Deque (combination Stack/Queue).
	public static class ListExtensions
    {
	    public static bool IsEmpty<T>(this List<T> list)
	    {
		    return list.Count == 0;
	    }
	    
		public static void Push<T>(this List<T> list, T item)
        {
            list.Add(item);
        }
		
		// Sometimes you just want to sneak something before the top.
		public static void InsertUnderTop<T>(this List<T> list, T item)
		{
			list.Add(item);
			(list[list.Count - 1], list[list.Count - 2]) = (list[list.Count - 2], list[list.Count - 1]);
		}

        public static T Pop<T>(this List<T> list)
        {
	        if (list.IsEmpty())
	        {
		        throw new System.Exception("Trying to pop from an empty list.");
	        }

	        T last = list[list.Count - 1];
	        list.RemoveAt(list.Count - 1);
	        return last;
        }

        public static T PeekInStack<T>(this List<T> list)
        {
	        return list[list.Count - 1];
        }

        public static T PeekInQueue<T>(this List<T> list)
        {
	        return list[0];
        }

        public static void Enqueue<T>(this List<T> list, T item)
        {
	        list.Add(item);
        }

        public static T Dequeue<T>(this List<T> list)
        {
            if (list.IsEmpty())
            {
				throw new System.Exception("Trying to dequeue from an empty list.");
			}

			T first = list[0];
            list.RemoveAt(0);
			return first;
        }

        public static string ToCommaSeparatedValuesString<T>(this List<T> list)
        {
	        var sb = new StringBuilder();
	        if (!list.IsEmpty())
	        {
		        sb.Append(list[0]);
		        for (int i = 1; i < list.Count; i++)
		        {
			        T item = list[i];
			        sb.Append($", {item}");
		        }
	        }

	        return sb.ToString();
	       
        }

        public static string ToNumberedItemsString<T>(this List<T> list, bool noLastNewLine = true)
        {
	        var sb = new StringBuilder();

	        if (!list.IsEmpty())
	        {
		        for (int i = 0; i < list.Count; i++)
		        {
			        T item = list[i];
			        bool lastLine = i == list.Count - 1;
			        string lineEnding = lastLine && noLastNewLine ? "" : "\n";
			        sb.Append($"{i}: {item}{lineEnding}");
		        }
	        }

	        return sb.ToString();
        }
    }
}