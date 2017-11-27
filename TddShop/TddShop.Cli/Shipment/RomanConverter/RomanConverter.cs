using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TddShop.Cli.Shipment
{
    //Standard roman notation ends on 4000
    //Notation with vinculum extends system
    //Here are sources for info on roman notations:
    //http://www.numericana.com/answer/roman.htm
    //https://en.wikipedia.org/wiki/Roman_numerals#Vinculum

    public class RomanConverter : IRomanConverter
    {
        IRomanLiterals _romanLiterals;

        public string Convert(int arabicValue)
        {
            _romanLiterals = arabicValue >= 4000 ? 
                RomanLiteralsFactory.createDictionaryInVinculumNotation() :
                                    RomanLiteralsFactory.createDictionaryInStandardNotation();
            
            bool validation = new RomanValidator(_romanLiterals).ValidateRoman(arabicValue);

            var convertionResult = ToRoman(arabicValue);

            // For less than 4000 change thousandSign to M
            convertionResult = arabicValue < 4000 ? convertionResult
                .Replace($"{_romanLiterals.thousandSign}", "M") :
                        convertionResult;
            
            return convertionResult
                .Replace($"C{_romanLiterals.thousandSign}", "CM");
        }

        public int Convert(string romanValue)
        {
            _romanLiterals =
                RomanLiteralsFactory.createDictionaryInVinculumNotation();

            //Case with "I vinculum M" which is "MM" || Standarize before conversion
            romanValue = romanValue.ToUpper().Replace("M", _romanLiterals.thousandSign);

            bool validation = new RomanValidator(_romanLiterals).ValidateRoman(romanValue);

            var convertionResult = FromRoman(romanValue);
            
            return convertionResult;
        }

        //TODO:Too many nestings, maybe use LINQ
        string ToRoman(int number)
        {
            var result = string.Empty;
            foreach (var characterNumber in _romanLiterals.dictionary.Keys)
            {
                while (number >= characterNumber)
                {
                    result += _romanLiterals.dictionary[characterNumber];
                    number -= characterNumber;
                }
            }
            return result;
        }

        //TODO:Too many nestings, maybe use LINQ
        int FromRoman(string inputString)
        {
            int result = 0;
            while (inputString.Length != 0)
            {
                foreach (var characterNumber in _romanLiterals.dictionary.Keys)
                {
                    string romanChar = _romanLiterals.dictionary[characterNumber];
                    if (inputString.StartsWith(romanChar, StringComparison.CurrentCulture))
                    {
                        inputString = inputString.Substring(romanChar.Length);
                        result += characterNumber;
                    }
                }
            }
            return result;
        }
    }
}
