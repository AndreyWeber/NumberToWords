using System;
using System.Text;

namespace NumberToWords
{
    /// <summary>
    /// Helper extesions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Append string to StringBuilder instance
        /// If instance Length > 0 add leading whitespace before appending string
        /// </summary>
        /// <param name="sb">StringBuilder instance</param>
        /// <param name="toAppend">String to append</param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder AppendWithLeadingWhitespace(this StringBuilder sb, String toAppend)
        {
            if (String.IsNullOrWhiteSpace(toAppend))
            {
                return sb;
            }

            return sb.Length == 0
                ? sb.Append(toAppend)
                : sb.Append($" {toAppend}");
        }

        /// <summary>
        /// Get symbol of string at specified position
        /// </summary>
        /// <param name="@string">String instance</param>
        /// <param name="position">Position in string</param>
        /// <returns>String</returns>
        public static String StrAtPos(this String @string, UInt16 position) =>
            position <= @string.Length - 1
                ? (@string.ToCharArray()[position] - '0').ToString()
                : throw new ArgumentOutOfRangeException(
                    nameof(position), $"Argument can't be greater, than argument '{nameof(@string)}' length = {@string.Length}");
    }
}