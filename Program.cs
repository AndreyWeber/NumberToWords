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

        public static void Main(string[] args)
        {
            // TODO: 1. Implement unit tests
            // TODO: 2. Optimize input data check - move to separate methods
            // TODO: 3. Optimize cases in switch ???
            // TODO: 4. Write comments
            // TODO: 5. Move parsing algorithm to separate class
            // TODO: 6. Move extesionsn to separate file
            // TODO: 7. Move input to app aruments

            const String rawInput = "0,25";

            var inputTokens = new String(rawInput.Where(c => !c.Equals(' ')).ToArray())
                .Split(',', StringSplitOptions.None);

            // Validate input value format
            if (inputTokens.Length == 0 || inputTokens.Length > 2)
            {
                Console.WriteLine($"ERROR! Invalid input number format: {rawInput}");
                Console.WriteLine("Application will stop now");
                return;
            }

            // Validate integer part
            var dollars = inputTokens[0];
            if (!Int32.TryParse(dollars, out var dRes))
            {
                Console.WriteLine($"ERROR! Invalid input number integer part format: {dollars}");
                Console.WriteLine("Application will stop now");
                return;
            }

            if (dollars.Length == 0 || dollars.Length > 9)
            {
                Console.WriteLine($"ERROR! Invalid input number integer part length: {dollars}");
                Console.WriteLine("Valid length from 1 up to 9 digits");
                Console.WriteLine("Application will stop now");
                return;
            }

            // Convert integer part to words
            var dollarWords = IntegerToWords(dollars);
            dollarWords = dollars.EndsWith("1") && !dollars.EndsWith("11")
                ? $"{dollarWords} dollar"
                : $"{dollarWords} dollars";

            // There is no decimal part. Execution finished
            if (inputTokens.Length < 2)
            {
                Console.WriteLine($"Output: {dollarWords}");
                return;
            }

            // Validate decimal part
            var cents = inputTokens[1].Equals("1") ? "10" : inputTokens[1];
            if (!Int32.TryParse(cents, out var cRes))
            {
                Console.WriteLine($"ERROR! Invalid input number decimal part format: {cents}");
                Console.WriteLine("Application will stop now");
                return;
            }

            if (cents.Length == 0 || cents.Length > 2)
            {
                Console.WriteLine($"ERROR! Invalid input number decimal part length: {cents}");
                Console.WriteLine("Valid length from 1 up to 2 digits");
                Console.WriteLine("Application will stop now");
                return;
            }

            var centWords = IntegerToWords(cents);
            centWords = cents.EndsWith("1") && !cents.EndsWith("11")
                ? $"{centWords} cent"
                : $"{centWords} cents";

            Console.WriteLine($"Output: {dollarWords} and {centWords}");

#if DEBUG
            // Console.ReadKey();
#endif
        }
    }
}
