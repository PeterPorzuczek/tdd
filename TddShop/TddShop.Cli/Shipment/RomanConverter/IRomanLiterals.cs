using System;
using System.Collections.Generic;

namespace TddShop.Cli
{
    public interface IRomanLiterals
    {
        Dictionary<int, string> dictionary { get; }
        string thousandSign { get; }
        string vinculumSign { get; }
        string vinculumDoubleSign { get; }
    }
}
