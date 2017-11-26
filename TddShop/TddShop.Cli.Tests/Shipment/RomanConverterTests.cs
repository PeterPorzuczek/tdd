using NUnit.Framework;
using System;
using TddShop.Cli.Shipment;

namespace TddShop.Cli.Tests.Shipment
{
    public class RomanConverterTests
    {

        /// <summary>
        /// Converts the value arabic value is proper converter should return expected roman value.
        /// Testing conversion from arabic to roman
        /// </summary>
        /// <param name="arabicValue">Arabic value.</param>
        /// <param name="romanValueExpected">Roman value expected.</param>

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
            var actual = romanNumerals.Convert(arabicValue);

            //Assert
            Assert.That(actual, Is.EqualTo(romanValueExpected));
        }

        /// <summary>
        /// Converts the value arabic value is proper and expected value is NOT Proper converter should return expected error.
        /// We test here vinculum notation use. Vinculum is used when our value is greater than 4000 because
        /// standard roman notation max value is 3999 and there shouldn't be 4 MMMM in a row
        /// </summary>
        /// <param name="arabicValue">Arabic value.</param>
        /// <param name="romanValueExpected">Roman value expected.</param>

        [TestCase(4000, "MMMM")] //Should be "I̅V̅"
        [TestCase(3000, "I̅I̅I̅")] //Should be "MMM"
        [TestCase(2000, "I̅I̅")] //Should be "MM"
        public void ConvertValue_ArabicValueIsProperAndExpectedValueIsNOTProper_ConverterShouldReturnNotExpected(int arabicValue, string romanValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var actual = romanNumerals.Convert(arabicValue);

            //Assert
            Assert.That(actual, !Is.EqualTo(romanValueExpected));
        }

        /// <summary>
        /// Converts the value arabic value is not proper converter should return exception.
        /// check for any bad ints theres no 0 in roman
        /// </summary>
        /// <param name="arabicValue">Arabic value.</param>

        [TestCase(-10)]
        [TestCase(0)]
        public void ConvertValue_ArabicValueIsNotProper_ConverterShouldReturnException(int arabicValue)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            TestDelegate exceptionDelegate = () => { romanNumerals.Convert(arabicValue); };

            //Assert
            Assert.Throws(typeof(RomanConverterNotNaturalNumberException), exceptionDelegate);
        }


        /// <summary>
        /// Converts the value roman value is proper converter should return expected arabic value.
        /// Testing conversion from roman to arabic
        /// </summary>
        /// <param name="romanValue">Roman value.</param>
        /// <param name="arabicValueExpected">Arabic value expected.</param>

        [TestCase("LII", 52)]
        [TestCase("I̅X̅", 9000)]
        [TestCase("MMM", 3000)]
        [TestCase("I̅I̅I̅", 3000)]
        public void ConvertValue_RomanValueIsProper_ConverterShouldReturnExpectedArabicValue(string romanValue, int arabicValueExpected)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            var actual = romanNumerals.Convert(romanValue);

            //Assert
            Assert.That(actual, Is.EqualTo(arabicValueExpected));
        }

        /// <summary>
        /// Converts the value roman value is not proper converter should return exception.
        /// Testing bad chars and bad roman numerals (not existing numerals like IXI)
        /// </summary>
        /// <param name="romanValue">Roman value.</param>

        [TestCase("IIV")]
        [TestCase("IXI")]
        [TestCase("IIL")]
        [TestCase("hyu")]
        public void ConvertValue_RomanValueIsNotProper_ConverterShouldReturnException(string romanValue)
        {
            //Arrange
            RomanConverter romanNumerals = new RomanConverter();

            //Act
            TestDelegate exceptionDelegate = () => { romanNumerals.Convert(romanValue); };

            //Assert
            Assert.Throws(typeof(RomanConverterInvalidInputException), exceptionDelegate);
        }
    }
}
