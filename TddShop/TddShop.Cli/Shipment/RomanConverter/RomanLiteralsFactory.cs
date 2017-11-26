using System;
namespace TddShop.Cli.Shipment
{
    public abstract class RomanLiteralsFactory
    {
        public static RomanLiterals createDictionaryInVinculumNotation() { return new RomanLiterals(true); }

        public static RomanLiterals createDictionaryInStandardNotation() { return new RomanLiterals(false); }
    }
}
