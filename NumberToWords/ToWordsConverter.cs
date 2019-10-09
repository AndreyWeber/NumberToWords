using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace NumberToWords
{
    /// <summary>
    /// Converter of numbers to word representation
    /// </summary>
    public static class ToWordsConverter
    {
        #region Private static fields

        private static IDictionary<String, String> units = new Dictionary<String, String>
        {
            { "0", "zero" },
            { "1", "one" },
            { "2", "two" },
            { "3", "three" },
            { "4", "four" },
            { "5", "five" },
            { "6", "six" },
            { "7", "seven" },
            { "8", "eight" },
            { "9", "nine" },
        };

        private static IDictionary<String, String> twoDigits = new Dictionary<String, String>
        {
            { "0", "" },
            { "10", "ten" },
            { "11", "eleven" },
            { "12", "twelve" },
            { "13", "thirteen" },
            { "14", "fourteen" },
            { "15", "fifteen" },
            { "16", "sixteen" },
            { "17", "seventeen" },
            { "18", "eighteen" },
            { "19", "nineteen" }
        };

        private static IDictionary<String, String> dozens = new Dictionary<String, String>
        {
            { "0", "" },
            { "2", "twenty" },
            { "3", "thirty" },
            { "4", "fourty" },
            { "5", "fifty" },
            { "6", "sixty" },
            { "7", "seventy" },
            { "8", "eighty" },
            { "9", "ninty" }
        };

        #endregion // Private static fields

        #region Public static methods

        /// <summary>
        /// Convert string representation of integer to word form
        /// </summary>
        /// <param name="intAsString" type="String">String representing integer</param>
        /// <returns>String representing integer in word form</returns>
        public static String IntToWords(String intAsString)
        {
            if (String.IsNullOrWhiteSpace(intAsString))
            {
                return String.Empty;
            }

            if (!Int32.TryParse(intAsString, out var res))
            {
                throw new ArgumentException(
                    $"Argument is not an Integer: {intAsString}", nameof(intAsString));
            }

            // Consider strings looking like "00", "000", etc. as 'zero'
            if (res == 0)
            {
                intAsString = "0";
            }

            var result = new StringBuilder();
            void Traverse(String token)
            {
                if (token.Equals(String.Empty))
                {
                    return;
                }

                switch (token.Length)
                {
                    case 1: // 1
                        result.AppendWithLeadingWhitespace(units[token]);
                        break;
                    case 2: // 10
                        if (token.StartsWith("0"))
                        {
                            Traverse(token.EndsWith("0")
                                ? token.Substring(2) // '00' cases
                                : token.Substring(1) // '0x' cases
                            );
                            break;
                        }

                        if (token.StartsWith("1"))
                        {
                            result.AppendWithLeadingWhitespace(twoDigits[token]);
                        }
                        else
                        {
                            result.AppendWithLeadingWhitespace(
                                token.EndsWith("0")
                                    ? dozens[token.StrAtPos(0)]
                                    : $"{dozens[token.StrAtPos(0)]}-{units[token.Last().ToString()]}"
                            );
                        }
                        break;
                    case 3 when !token.StartsWith("0"): // 100
                        result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred");
                        Traverse(token.Substring(1));
                        break;
                    case 4 when !token.StartsWith("0"): // 1 000
                        result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} thousand");
                        Traverse(token.Substring(1));
                        break;
                    case 5 when !token.StartsWith("0"): // 10 000
                        if (token.StartsWith("1"))
                        {
                            result.AppendWithLeadingWhitespace($"{twoDigits[token.Substring(0, 2)]} thousand");
                        }
                        else
                        {
                            result.AppendWithLeadingWhitespace(
                                token.StrAtPos(1).Equals("0")
                                    ? $"{dozens[token.StrAtPos(0)]} thousand"
                                    : $"{dozens[token.StrAtPos(0)]}-{units[token.StrAtPos(1)]} thousand"
                            );
                        }
                        Traverse(token.Substring(2)); // move to hundreds
                        break;
                    case 6 when !token.StartsWith("0"): // 100 000
                        if (token.StrAtPos(1).Equals("0") && token.StrAtPos(2).Equals("0")) // '100 xxx' cases
                        {
                            result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred thousand");
                            Traverse(token.Substring(2)); // move to thousands
                            break;
                        }

                        result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred");
                        Traverse(token.Substring(1));
                        break;
                    case 7 when !token.StartsWith("0"): // 1 000 000
                        result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} million");
                        Traverse(token.Substring(1));
                        break;
                    case 8 when !token.StartsWith("0"): // 10 000 000
                        if (token.StartsWith("1"))
                        {
                            result.AppendWithLeadingWhitespace($"{twoDigits[token.Substring(0, 2)]} million");
                        }
                        else
                        {
                            result.AppendWithLeadingWhitespace(
                                token.StrAtPos(1).Equals("0")
                                    ? $"{dozens[token.StrAtPos(0)]} million"
                                    : $"{dozens[token.StrAtPos(0)]}-{units[token.StrAtPos(1)]} million"
                            );
                        }
                        Traverse(token.Substring(2)); // move to millions
                        break;
                    case 9 when !token.Equals("0"): // 100 000 000
                        if (token.StrAtPos(1).Equals("0") && token.StrAtPos(2).Equals("0")) // '100 xxx xxx' cases
                        {
                            result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred million");
                            Traverse(token.Substring(2)); // move to hundred thousands
                            break;
                        }

                        result.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred");
                        Traverse(token.Substring(1));
                        break;
                    default:
                        Traverse(token.Substring(1));
                        break;
                }
            }

            Traverse(intAsString);

            return result.ToString();
        }

        #endregion // Public static methods
    }
}