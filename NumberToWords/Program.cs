using System;
using System.Linq;

using static System.Console;
using static System.Environment;

namespace NumberToWords
{
    public class Program
    {
        #region Helper methods

        /// <summary>
        /// Write colored string to standard System.Console output
        /// </summary>
        /// <param name="message">Message to write to console output</param>
        /// <param name="color">Message color</param>
        private static void ConsoleColoredWrite(String message, ConsoleColor? color = null)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (!color.HasValue)
            {
                Write(message);
            }

            var originalColor = ForegroundColor;
            ForegroundColor = color.Value;
            Write(message);
            ForegroundColor = originalColor;
        }

        /// <summary>
        /// Display utility usage
        /// </summary>
        private static void ShowUsage()
        {
            WriteLine("Utililty will convert number representing sum in dollars to a word representation");
            WriteLine("\t- Maximum integer part of number: 999 999 999");
            WriteLine("\t- Maximum decimal part of number: 99");
            WriteLine("\t- Decimal separator: comma ','");
            WriteLine("\t- Digits can be separated by whitespace(s)");
            Write($"{NewLine}Type ");
            ConsoleColoredWrite("[Q]uit", ConsoleColor.Yellow);
            WriteLine($" to exit{NewLine}");
            WriteLine($"Please provide number{NewLine}");
        }

        /// <summary>
        /// Try to parse input string and get integer and decimal parts of
        /// number string representation
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="integer">Output integer part</param>
        /// <param name="cents">Output decimal part</param>
        private static void TryParseInput(String input, out String integer, out String cents)
        {
            integer = String.Empty;
            cents = String.Empty;

            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ApplicationException("Error! Empty input. Try again please");
            }

            // Attempt to divide input string on integer and decimal parts
            var inputTokens = new String(input.Where(c => !c.Equals(' ')).ToArray())
                .Split(',', StringSplitOptions.None);

            // Validate input format
            if (inputTokens.Length > 2)
            {
                throw new ApplicationException($"Error! Invalid input number format: '{input}'");
            }

            // Validate integer part
            integer = inputTokens[0].All(Char.IsDigit)
                ? inputTokens[0]
                : throw new ApplicationException($"Error! Invalid input number integer part format: '{inputTokens[0]}'");

            if (integer.Length == 0 || integer.Length > 9)
            {
                throw new ApplicationException($"Error! Invalid input number integer part length: '{integer}'" +
                    " Valid length is from 1 up to 9 digits");
            }

            // Check if decimal part exist
            if (inputTokens.Length < 2)
            {
                return;
            }

            // Validate decimal part
            cents = inputTokens[1].Equals("1") ? "10" : inputTokens[1];
            if (!cents.All(Char.IsDigit))
            {
                throw new ApplicationException($"Error! Invalid input number decimal part format: '{cents}'");
            }

            if (cents.Length == 0 || cents.Length > 2)
            {
                throw new ApplicationException($"Error! Invalid input number decimal part length: '{cents}'" +
                    " Valid length from 1 up to 2 digits");
            }
        }

        #endregion // Helper methods

        public static void Main(string[] args)
        {
            ShowUsage();

            while (true)
            {
                Write("Input: ");
                var rawInput = Console.ReadLine();
                Write(NewLine);

                if (rawInput.Equals("q", StringComparison.InvariantCultureIgnoreCase) ||
                    rawInput.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                try
                {
                    TryParseInput(rawInput, out var dollars, out var cents);

                    // Convert integer part to words
                    var dollarWords = ToWordsConverter.IntToWords(dollars);
                    dollarWords = dollars.EndsWith("1") && !dollars.EndsWith("11")
                        ? $"{dollarWords} dollar"
                        : $"{dollarWords} dollars";

                    // There is no decimal part. Show output and continue execution
                    if (String.IsNullOrWhiteSpace(cents))
                    {
                        ConsoleColoredWrite($"Output: {dollarWords}", ConsoleColor.Green);
                        WriteLine(NewLine);
                        continue;
                    }

                    // Convert decimal part to words
                    var centWords = ToWordsConverter.IntToWords(cents);
                    centWords = cents.EndsWith("1") && !cents.EndsWith("11")
                        ? $"{centWords} cent"
                        : $"{centWords} cents";

                    ConsoleColoredWrite($"Output: {dollarWords} and {centWords}", ConsoleColor.Green);
                    WriteLine(NewLine);
                }
                catch (Exception ex)
                {
                    ConsoleColoredWrite(ex.Message, ConsoleColor.Red);
                    WriteLine(NewLine);
                }
            }
#if DEBUG
            WriteLine("Press any key to close utility");
            ReadKey(true);
#endif
        }
    }
}
