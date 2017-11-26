using System;

namespace TddShop.Cli.Shipment
{
    public class RomanConverterException : Exception
    {
        const string _defaultMessage = "Something went wrong";

        public RomanConverterException()
            : base(_defaultMessage) => Console.WriteLine(_defaultMessage);

        public RomanConverterException(string message)
            : base(message) => Console.WriteLine(message);
    }

    public class RomanConverterNotNaturalNumberException : RomanConverterException
    {
        const string _defaultMessage = "Value is not a natural number";

        public RomanConverterNotNaturalNumberException() 
            : base(_defaultMessage) => Console.WriteLine(_defaultMessage);
    }

    public class RomanConverterInvalidInputException : RomanConverterException
    {
        const string _defaultMessage = "Value is not a valid roman numeral";

        public RomanConverterInvalidInputException()
            : base(_defaultMessage) => Console.WriteLine(_defaultMessage);
    }
}
