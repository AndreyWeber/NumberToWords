using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumberToWords.Tests
{
    [TestClass]
    public class ToWordsConverterTest
    {
        [DataTestMethod]
        [DataRow(null, "", DisplayName = "Null string")]
        [DataRow("", "", DisplayName = "Empty string")]
        [DataRow("0", "zero", DisplayName = "Single zero")]
        [DataRow("00", "zero", DisplayName = "Double zero")]
        [DataRow("000000", "zero", DisplayName = "Multiple zero")]
        [DataRow("1", "one")]
        [DataRow("10", "ten")]
        [DataRow("11", "eleven")]
        [DataRow("20", "twenty")]
        [DataRow("25", "twenty-five")]
        [DataRow("100", "one hundred")]
        [DataRow("101", "one hundred one")]
        [DataRow("111", "one hundred eleven")]
        [DataRow("120", "one hundred twenty")]
        [DataRow("125", "one hundred twenty-five")]
        [DataRow("1000", "one thousand")]
        [DataRow("1001", "one thousand one")]
        [DataRow("1010", "one thousand ten")]
        [DataRow("1011", "one thousand eleven")]
        [DataRow("1020", "one thousand twenty")]
        [DataRow("1025", "one thousand twenty-five")]
        [DataRow("1100", "one thousand one hundred")]
        [DataRow("1101", "one thousand one hundred one")]
        [DataRow("1110", "one thousand one hundred ten")]
        [DataRow("1111", "one thousand one hundred eleven")]
        [DataRow("1020", "one thousand twenty")]
        [DataRow("1125", "one thousand one hundred twenty-five")]
        [DataRow("10000", "ten thousand")]
        [DataRow("10001", "ten thousand one")]
        [DataRow("10010", "ten thousand ten")]
        [DataRow("10011", "ten thousand eleven")]
        [DataRow("10020", "ten thousand twenty")]
        [DataRow("10025", "ten thousand twenty-five")]
        [DataRow("10100", "ten thousand one hundred")]
        [DataRow("10101", "ten thousand one hundred one")]
        [DataRow("10110", "ten thousand one hundred ten")]
        [DataRow("10111", "ten thousand one hundred eleven")]
        [DataRow("10120", "ten thousand one hundred twenty")]
        [DataRow("10125", "ten thousand one hundred twenty-five")]
        [DataRow("11000", "eleven thousand")]
        [DataRow("20000", "twenty thousand")]
        [DataRow("25000", "twenty-five thousand")]
        [DataRow("25125", "twenty-five thousand one hundred twenty-five")]
        [DataRow("100000", "one hundred thousand")]
        [DataRow("100001", "one hundred thousand one")]
        [DataRow("110000", "one hundred ten thousand")]
        [DataRow("111000", "one hundred eleven thousand")]
        [DataRow("111100", "one hundred eleven thousand one hundred")]
        [DataRow("111110", "one hundred eleven thousand one hundred ten")]
        [DataRow("111111", "one hundred eleven thousand one hundred eleven")]
        [DataRow("125125", "one hundred twenty-five thousand one hundred twenty-five")]
        [DataRow("1000000", "one million")]
        [DataRow("1000001", "one million one")]
        [DataRow("1100000", "one million one hundred thousand")]
        [DataRow("1110000", "one million one hundred ten thousand")]
        [DataRow("1111000", "one million one hundred eleven thousand")]
        [DataRow("1111100", "one million one hundred eleven thousand one hundred")]
        [DataRow("1111110", "one million one hundred eleven thousand one hundred ten")]
        [DataRow("1111111", "one million one hundred eleven thousand one hundred eleven")]
        [DataRow("9999999", "nine million nine hundred ninty-nine thousand nine hundred ninty-nine")]
        [DataRow("10000000", "ten million")]
        [DataRow("10000001", "ten million one")]
        [DataRow("11000000", "eleven million")]
        [DataRow("20000000", "twenty million")]
        [DataRow("25000000", "twenty-five million")]
        [DataRow("99999999", "ninty-nine million nine hundred ninty-nine thousand nine hundred ninty-nine")]
        [DataRow("100000000", "one hundred million")]
        [DataRow("100000001", "one hundred million one")]
        [DataRow("110000000", "one hundred ten million")]
        [DataRow("111000000", "one hundred eleven million")]
        [DataRow("999999999", "nine hundred ninty-nine million nine hundred ninty-nine thousand nine hundred ninty-nine")]
        public void IntToWords_IntAsString_ReturnsIntAsWordString(String number, String expectedWords)
        {
            var actualWords = ToWordsConverter.IntToWords(number);

            Assert.AreEqual(expectedWords, actualWords, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntoToWords_NotIntAsString_ThrowsArgumentException()
        {
            var argument = "Some string";

            ToWordsConverter.IntToWords(argument);
        }
    }
}
