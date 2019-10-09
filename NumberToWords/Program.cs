using System;
using System.Linq;

namespace NumberToWords
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: 1. Implement unit tests
            // TODO: 2. Optimize input data check - move to separate methods
            // TODO: 3. Optimize cases in switch ???
            // TODO: 4. Write comments
            // TODO: 6. Move input to app aruments

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
            var dollarWords = WordConverter.IntNumToWords(dollars);
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

            var centWords = WordConverter.IntNumToWords(cents);
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
