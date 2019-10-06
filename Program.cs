using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NumberToWords
{
    public class Program
    {
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

        private static String IntegerToWords(String input)
        {
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

            Traverse(input);

            return result.ToString();
        }

        private static String DecimalToWords(String input) => String.Empty;

        public static void Main(string[] args)
        {
            // TODO: 1. Implement unit tests
            // TODO: 2. Implement input data check
            // TODO: 3. Implement dividing on dollars and cents
            // TODO: 4. Implement cents parsing
            // TODO: 5. Implement adding 'dollar(s)', 'cent(s)' suffixes
            // TODO: 6. Optimize cases in switch ???

            //! Length > 0 && Length <= 9 - for Int part
            //! Length >= 1 && Length <= 2 - for Decimal part

            const String rawInput = "286010001";

            var input = new String(rawInput.Where(c => !c.Equals(' ')).ToArray());

            var words = IntegerToWords(input);
            Console.WriteLine(words);

            // Console.ReadKey();
        }
    }

    public static class Extensions
    {
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

        public static String StrAtPos(this String @string, UInt16 position) =>
            position <= @string.Length - 1
                ? (@string.ToCharArray()[position] - '0').ToString()
                : throw new ArgumentOutOfRangeException(
                    nameof(position), $"Argument can't be greater, than argument '{nameof(@string)}' length = {@string.Length}");
    }
}
