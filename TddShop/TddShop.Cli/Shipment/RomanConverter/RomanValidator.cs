using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TddShop.Cli.Shipment
{
    public class RomanValidator
    {
        readonly IRomanLiterals _romanLiterals;

        public RomanValidator(IRomanLiterals romanLiterals)
        {
            _romanLiterals = romanLiterals;
        }

        public bool ValidateRoman(string text)
        {
            string separator = _romanLiterals.vinculumSign;
            if (text.Contains(separator))
            {
                if (!CheckVinculumText(text, separator)) { throw new RomanConverterInvalidInputException(); }
            }
            else
            {
                if (!TestStandardRoman(text)) { throw new RomanConverterInvalidInputException(); }
            }
            return true;
        }

        public bool ValidateRoman(int num)
        {
            if (num < 1)
            {
                throw new RomanConverterNotNaturalNumberException();
            }
            return true;
        }

        bool TestStandardRoman(string text)
        {
            Regex regex = new Regex(@"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");
            return regex.IsMatch(text);
        }

        bool TestVinculumRoman(string text)
        {
            Regex regex = new Regex(@"^I{0,4}(CI|CD|D?C{0,4})(XC|XL|L?X{0,4})(IX|IV|V?I{0,4})$");
            return regex.IsMatch(text.Replace(_romanLiterals.vinculumSign, ""));
        }

        bool CheckVinculumText(string text, string separator)
        {
            string[] vinculumSeparated = SplitVinculum(text, separator);
            return
                TestVinculumRoman(vinculumSeparated[0]) &&
                TestStandardRoman(vinculumSeparated[1]) ?
                    true : false;
        }

        string[] SplitVinculum(string text, string separator)
        {
            string[] split = SeparateByText(text, separator);
            string vinculumPart = string.Join(separator, split.Take(split.Length - 1))
                                + separator;
            string standardPart = split.Last();
            return new string[] { vinculumPart, standardPart };
        }

        string[] SeparateByText(string text, string separator)
        {
            string[] stringSeparator = new string[] { separator };
            string[] result = text
                            .Split(stringSeparator,
                                StringSplitOptions.None);
            return result;
        }
    }
}
