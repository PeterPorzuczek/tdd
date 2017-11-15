using System;
using System.Collections.Generic;
using System.Linq;

namespace TddShop.Cli.Shipment
{
    //Main part
    public partial class RomanConverter : IRomanConverter
    {
        private Dictionary<int, string> _dictionary;
        private string _thousandSign;
        private string _visculumSign = "\u0305";
        private string _visculumDoubleSign = "\u0305\u0305";

        //TODO:Too big
        public Tuple<string, int> Convert(int arabicValue)
        {
            bool isVisculum = arabicValue >= 4000 ? true : false;
            _thousandSign = visculumThousandSign(isVisculum);
            _dictionary = createDictionary(isVisculum);

            var convertionResult = toRoman(arabicValue);
            var validator = fromRoman(convertionResult);

            if (validator == arabicValue && validator != 0)
            {
                // For less than 4000 change thousandSign to M
                convertionResult = arabicValue < 4000 ? convertionResult
                                        .Replace($"{_thousandSign}", "M") :
                                                    convertionResult;
                return Tuple.Create(convertionResult
                                    .Replace($"C{_thousandSign}", "CM")
                                    , arabicValue);
            }
            else
            {
                return Tuple.Create("error", 0);
            }
        }

        private Boolean checkAllowableChars(string romanValue)
        {
            string allowableChars = getAllowableChars(_dictionary);
            return romanValue.Replace($"{_visculumSign}", "")
                             .All(c => allowableChars.Contains(c));
        }

        //TODO:Too big
        public Tuple<string, int> Convert(string romanValue)
        {
            bool isVisculum = true;
            _thousandSign = visculumThousandSign(isVisculum);
            _dictionary = createDictionary(isVisculum);

            //Case with "I vinculum M" which is "MM" or "I\u0305I\u0305"
            romanValue = romanValue.ToUpper().Replace("M", _thousandSign);

            //Check string if contains chars from dictionary
            bool charsCheck = checkAllowableChars(romanValue);

            var convertionResult = charsCheck ? fromRoman(romanValue) : 0;
            var validator = toRoman(convertionResult);

            //Validate if conversion is ok
            if (validator == romanValue && convertionResult != 0)
            {
                // For less than 4000 change thousandSign to M
                romanValue = convertionResult < 4000 ? romanValue
                                        .Replace($"{_thousandSign}", "M") :
                                                 romanValue;
                return Tuple.Create(romanValue, convertionResult);
            }
            else
            {
                return Tuple.Create("error", 0);
            }
        }

    }

    //Converter internal part
    partial class RomanConverter
    {
        //TODO:Too many nestings, maybe use LINQ
        private string toRoman(int number)
        {
            var result = string.Empty;
            foreach (var characterNumber in _dictionary.Keys)
            {
                while (number >= characterNumber)
                {
                    result += _dictionary[characterNumber];
                    number -= characterNumber;
                }
            }
            return result;
        }

        //TODO:Too many nestings, maybe use LINQ
        private int fromRoman(string inputString)
        {
            int result = 0;
            while (inputString.Length != 0)
            {
                foreach (var characterNumber in _dictionary.Keys)
                {
                    string romanChar = _dictionary[characterNumber];
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

    //Dictionary part
    partial class RomanConverter
    {
        private Dictionary<int, string> createDictionary(Boolean isVisulum)
        {
            Dictionary<int, string> visculum = romanVisulumCharacters();
            Dictionary<int, string> standard = romanStandardCharacters();
            standard.ToList().ForEach(kvp => visculum.Add(kvp.Key, kvp.Value));
            return isVisulum ? visculum : standard;
        }

        private Dictionary<int, string> romanStandardCharacters()
        {
            return new Dictionary<int, string>
            {
                {1000,  $"{_thousandSign}"      },
                {900,   $"C{_thousandSign}"     },
                {500,   "D"                     },
                {400,   "CD"                    },
                {100,   "C"                     },
                {90,    "XC"                    },
                {50,    "L"                     },
                {40,    "XL"                    },
                {10,    "X"                     },
                {9,     "IX"                    },
                {5,     "V"                     },
                {4,     "IV"                    },
                {1,     "I"                     }
             };
        }

        private Dictionary<int, string> romanVisulumCharacters()
        {
            return new Dictionary<int, string>
            {
                {1000000000,    $"{_thousandSign}{_visculumDoubleSign}" },
                {500000000,     $"D{_visculumDoubleSign}"               },
                {100000000,     $"C{_visculumDoubleSign}"               },
                {50000000,      $"L{_visculumDoubleSign}"               },
                {10000000,      $"X{_visculumDoubleSign}"               },
                {5000000,       $"V{_visculumDoubleSign}"               },
                {1000000,       $"{_thousandSign}{_visculumSign}"       },
                {500000,        $"D{_visculumSign}"                     },
                {100000,        $"C{_visculumSign}"                     },
                {50000,         $"L{_visculumSign}"                     },
                {10000,         $"X{_visculumSign}"                     },
                {9000,          $"I{_visculumSign}X{_visculumSign}"     },
                {8000,          $"V{_visculumSign}" +
                                $"I{_visculumSign}" +
                                $"I{_visculumSign}" +
                                $"I{_visculumSign}"                     },
                {7000,          $"V{_visculumSign}" +
                                $"I{_visculumSign}" +
                                $"I{_visculumSign}"                     },
                {6000,          $"V{_visculumSign}I{_visculumSign}"     },
                {5000,          $"V{_visculumSign}"                     },
                {4000,          $"I{_visculumSign}V{_visculumSign}"     },
             };
        }

        private string visculumThousandSign(Boolean isVisulum)
        {
            return isVisulum ? $"I{_visculumSign}" : "M";
        }

        private string getAllowableChars(Dictionary<int, string> dictionary)
        {
            string allowableLetters = string.Empty;
            dictionary.ToList().ForEach(kvp => allowableLetters += kvp.Value);
            return new string(allowableLetters.Replace($"{_visculumSign}", "")
                              .ToCharArray().Distinct()
                              .ToArray());
        }
    }
}
