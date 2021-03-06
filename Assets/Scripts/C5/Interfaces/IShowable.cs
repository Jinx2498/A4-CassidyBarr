using System;
using System.Text;

#pragma warning disable 8632

namespace C5
{
    /// <summary>
    /// <i>(Describe usage of "L:300" format string.)</i>
    /// </summary>
    public interface IShowable : IFormattable
    {
        //TODO: wonder if we should use TextWriters instead of StringBuilders?
        /// <summary>
        /// Format <code>this</code> using at most approximately <code>rest</code> chars and 
        /// append the result, possibly truncated, to stringBuilder.
        /// Subtract the actual number of used chars from <code>rest</code>.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="rest"></param>
        /// <param name="formatProvider"></param>
        /// <returns>True if the appended formatted string was complete (not truncated).</returns>
        bool Show(StringBuilder stringBuilder, ref int rest, IFormatProvider? formatProvider);
    }
}