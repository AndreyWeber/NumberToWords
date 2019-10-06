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
            var result = String.Empty;
            var sb = new StringBuilder();
            void Traverse(String token)
            {
                if (token.Equals(String.Empty))
                {
                    return;
                }

                switch (token.Length)
                {
                    case 1: // 1
                        result = result.Equals(String.Empty)
                            ? units[token]
                            : $"{result} {units[token]}";
                        sb.AppendWithLeadingWhitespace(units[token]);
                        break;
                    case 2: // 10
                        if (token.StartsWith("0"))
                        {
                            if (token.EndsWith("0")) // '00' cases
                            {
                                Traverse(token.Substring(2));
                            }
                            else // '0x' cases
                            {
                                Traverse(token.Substring(1));
                            }
                            break;
                        }

                        if (token.StartsWith("1"))
                        {
                            result = result.Equals(String.Empty)
                                ? twoDigits[token]
                                : $"{result} {twoDigits[token]}";
                            sb.AppendWithLeadingWhitespace(twoDigits[token]);
                        }
                        else
                        {
                            if (token.EndsWith("0"))
                            {
                                result = result.Equals(String.Empty)
                                    ? dozens[token.StrAtPos(0)]
                                    : $"{result} {dozens[token.StrAtPos(0)]}";
                                sb.AppendWithLeadingWhitespace(dozens[token.StrAtPos(0)]);
                            }
                            else
                            {
                                result = result.Equals(String.Empty)
                                    ? $"{dozens[token.StrAtPos(0)]}-{units[token.Last().ToString()]}"
                                    : $"{result} {dozens[token.StrAtPos(0)]}-{units[token.Last().ToString()]}";
                                sb.AppendWithLeadingWhitespace(
                                    $"{dozens[token.StrAtPos(0)]}-{units[token.Last().ToString()]}");
                            }
                        }
                        break;
                    case 3 when !token.StartsWith("0"): // 100
                        // if (token.StartsWith("0"))
                        // {
                        //     Traverse(token.Substring(1));
                        //     break;
                        // }

                        result = result.Equals(String.Empty)
                                ? $"{units[token.StrAtPos(0)]} hundred"
                                : $"{result} {units[token.StrAtPos(0)]} hundred";
                        sb.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred");
                        Traverse(token.Substring(1));
                        break;
                    case 4 when !token.StartsWith("0"): // 1 000
                        // if (token.StartsWith("0"))
                        // {
                        //     Traverse(token.Substring(1));
                        //     break;
                        // }

                        result = result.Equals(String.Empty)
                            ? $"{units[token.StrAtPos(0)]} thousand"
                            : $"{result} {units[token.StrAtPos(0)]} thousand";
                        sb.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} thousand");
                        Traverse(token.Substring(1));
                        break;
                    case 5: // 10 000
                        if (token.StartsWith("0"))
                        {
                            Traverse(token.Substring(1));
                            break;
                        }

                        if (token.StartsWith("1"))
                        {
                            result = result.Equals(String.Empty)
                                ? $"{twoDigits[token.Substring(0, 2)]} thousand"
                                : $"{result} {twoDigits[token.Substring(0, 2)]} thousand";
                            sb.AppendWithLeadingWhitespace($"{twoDigits[token.Substring(0, 2)]} thousand");
                        }
                        else
                        {
                            if (token.StrAtPos(1).Equals("0"))
                            {
                                result = result.Equals(String.Empty)
                                    ? $"{dozens[token.StrAtPos(0)]} thousand"
                                    : $"{result} {dozens[token.StrAtPos(0)]} thousand";
                                sb.AppendWithLeadingWhitespace($"{dozens[token.StrAtPos(0)]} thousand");
                            }
                            else
                            {
                                result = result.Equals(String.Empty)
                                    ? $"{dozens[token.StrAtPos(0)]}-{units[token.StrAtPos(1)]} thousand"
                                    : $"{result} {dozens[token.StrAtPos(0)]}-{units[token.StrAtPos(1)]} thousand";
                                sb.AppendWithLeadingWhitespace(
                                    $"{dozens[token.StrAtPos(0)]}-{units[token.StrAtPos(1)]} thousand");
                            }
                        }
                        Traverse(token.Substring(2)); // move to hundreds
                        break;
                    case 6: // 100 000
                        if (token.StartsWith("0"))
                        {
                            Traverse(token.Substring(1));
                            break;
                        }

                        if (token.StrAtPos(1).Equals("0") && token.StrAtPos(2).Equals("0")) // '100 xxx' cases
                        {
                            result = result.Equals(String.Empty)
                                ? $"{units[token.StrAtPos(0)]} hundred thousand"
                                : $"{result} {units[token.StrAtPos(0)]} hundred thousand";
                            sb.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred thousand");
                            Traverse(token.Substring(2));
                            break;
                        }

                        result = result.Equals(String.Empty)
                                ? $"{units[token.StrAtPos(0)]} hundred"
                                : $"{result} {units[token.StrAtPos(0)]} hundred";
                        sb.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} hundred");
                        Traverse(token.Substring(1));
                        break;
                    case 7: // 1 000 000
                        if (token.StartsWith("0"))
                        {
                            Traverse(token.Substring(1));
                            break;
                        }

                        result = result.Equals(String.Empty)
                                ? $"{units[token.StrAtPos(0)]} million"
                                : $"{result} {units[token.StrAtPos(0)]} million";
                        sb.AppendWithLeadingWhitespace($"{units[token.StrAtPos(0)]} million");
                        Traverse(token.Substring(1));
                        break;
                    // case 8: // 10 000 000
                    //     break;
                    // case 9: // 100 000 000
                    //     break;
                    default:
                        // throw new ArgumentOutOfRangeException(
                        //     nameof(input), input.Length, "Integer part is out of range");
                        Traverse(token.Substring(1));
                        break;
                }
            }

            Traverse(input);
            var sbStr = sb.ToString();
            return result;
        }

        private static String DecimalToWords(String input) => String.Empty;

        public static void Main(string[] args)
        {
            // TODO: 1. Optimization
            // TODO: 2. Use StringBuilder. Extension method AppendWithLeadingWhitespace()
            // TODO: 3. Implement unit tests
            // TODO: 4. Implement input data check
            // TODO: 5. Implement dividing on dollars and cents
            // TODO: 6. Implement cents parsing
            // TODO: 7. Implement adding 'dollar(s)', 'cent(s)' suffixes

            //! Length > 0 && Length <= 9 - for Int part
            //! Length >= 1 && Length <= 2 - for Decimal part

            const String rawInput = "7110011";

            var input = new String(rawInput.Where(c => !c.Equals(' ')).ToArray());

            Console.WriteLine(IntegerToWords(input));

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
