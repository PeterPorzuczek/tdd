using System;
using System.Collections.Generic;
using System.Linq;

namespace TddShop.Cli.Shipment
{
    public class RomanLiterals : IRomanLiterals
    {
        public Dictionary<int, string> dictionary { get; }

        public string thousandSign => _thousandSign;
        public string vinculumSign => _vinculumSign;
        public string vinculumDoubleSign => _vinculumSign;

        private string _thousandSign;
        private const string _vinculumSign = "\u0305";
        private const string _vinculumDoubleSign = "\u0305\u0305";

        public RomanLiterals(Boolean isVinculum)
        {
            _thousandSign = VinculumThousandSign(isVinculum);
            dictionary = CreateDictionary(isVinculum);
        }

        private Dictionary<int, string> CreateDictionary(Boolean isVinculum)
        {
            Dictionary<int, string> vinculum = RomanVinculumCharacters();
            Dictionary<int, string> standard = RomanStandardCharacters();
            standard.ToList().ForEach(kvp => vinculum.Add(kvp.Key, kvp.Value));
            return isVinculum ? vinculum : standard;
        }

        private Dictionary<int, string> RomanStandardCharacters()
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

        private Dictionary<int, string> RomanVinculumCharacters()
        {
            return new Dictionary<int, string>
            {
                {1000000000,    $"{_thousandSign}{_vinculumDoubleSign}" },
                {500000000,     $"D{_vinculumDoubleSign}"               },
                {100000000,     $"C{_vinculumDoubleSign}"               },
                {50000000,      $"L{_vinculumDoubleSign}"               },
                {10000000,      $"X{_vinculumDoubleSign}"               },
                {5000000,       $"V{_vinculumDoubleSign}"               },
                {1000000,       $"{_thousandSign}{_vinculumSign}"       },
                {500000,        $"D{_vinculumSign}"                     },
                {100000,        $"C{_vinculumSign}"                     },
                {50000,         $"L{_vinculumSign}"                     },
                {10000,         $"X{_vinculumSign}"                     },
                {9000,          $"I{_vinculumSign}X{_vinculumSign}"     },
                {8000,          $"V{_vinculumSign}" +
                                $"I{_vinculumSign}" +
                                $"I{_vinculumSign}" +
                                $"I{_vinculumSign}"                     },
                {7000,          $"V{_vinculumSign}" +
                                $"I{_vinculumSign}" +
                                $"I{_vinculumSign}"                     },
                {6000,          $"V{_vinculumSign}I{_vinculumSign}"     },
                {5000,          $"V{_vinculumSign}"                     },
                {4000,          $"I{_vinculumSign}V{_vinculumSign}"     },
             };
        }

        private string VinculumThousandSign(Boolean isVinculum)
        {
            return isVinculum ? $"I{_vinculumSign}" : "M";
        }
    }
}
