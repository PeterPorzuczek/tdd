using NUnit.Framework;
using System;
using TddShop.Cli.Shipment;

namespace TddShop.Cli.Tests.Shipment
{
    public class RomanConverterTests
    {
        [TestCase(4000, "I̅V̅")]
        [TestCase(3000, "MMM")]
        [TestCase(18034, "X̅V̅I̅I̅I̅XXXIV")]
        [TestCase(3034, "MMMXXXIV")]
        [TestCase(10, "X")]
        [TestCase(2000, "MM")]
        public void ConvertValue_ArabicValueIsProper_ConverterShouldReturnExpectedRomanValue(int arabicValue, string romanValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var result = romanNumerals.Convert(arabicValue);

            //Assert
            Assert.That(result.Item1, Is.EqualTo(romanValueExpected));
        }

        /// <summary>
        /// Converts the value arabic value is proper and expected value is NOT Proper converter should return expected error.
        /// We test here visculum notation use. Visculum is used when our value is greater than 4000 because
        /// standard roman notation max value is 4000 and there shouldn't be 4 MMMM in row but some romans 
        /// use standard so we allow to convert MMMM but converter always use visculum notation 
        /// for returning more than 4000
        /// </summary>
        /// <param name="arabicValue">Arabic value.</param>
        /// <param name="romanValueExpected">Roman value expected.</param>

        [TestCase(4000, "MMMM")]
        [TestCase(3000, "I̅I̅I̅")]
        [TestCase(2000, "I̅I̅")]
        public void ConvertValue_ArabicValueIsProperAndExpectedValueIsNOTProper_ConverterShouldReturnExpectedERROR(int arabicValue, string romanValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var result = romanNumerals.Convert(arabicValue);

            //Assert
            Assert.That(result.Item1, !Is.EqualTo(romanValueExpected));
        }

        [TestCase(0, "error")]
        public void ConvertValue_ArabicValueIsNOTProper_ConverterShouldReturnExpectedERROR(int arabicValue, string romanValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var result = romanNumerals.Convert(arabicValue);

            //Assert
            Assert.That(result.Item1, Is.EqualTo(romanValueExpected));
        }

        [TestCase("LII", 52)]
        [TestCase("MMMMMMMMM", 9000)]
        [TestCase("I̅I̅I̅I̅I̅I̅I̅I̅I̅", 9000)]
        [TestCase("I̅X̅", 9000)]
        [TestCase("MMM", 3000)]
        [TestCase("I̅I̅I̅", 3000)]
        public void ConvertValue_RomanValueIsProper_ConverterShouldReturnExpectedArabicValue(string romanValue, int arabicValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var result = romanNumerals.Convert(romanValue);

            //Assert
            Assert.That(result.Item2, Is.EqualTo(arabicValueExpected));
        }

        [TestCase("hyu", "error")]
        public void ConvertValue_RomanValueIsNOTProper_ConverterShouldReturnExpectedERROR(string romanValue, string romanErrorValue)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var result = romanNumerals.Convert(romanValue);

            //Assert
            Assert.That(result.Item1, Is.EqualTo(romanErrorValue));
        }
    }
}
