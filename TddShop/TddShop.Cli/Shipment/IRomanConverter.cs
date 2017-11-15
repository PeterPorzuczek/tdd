using System;
namespace TddShop.Cli.Shipment
{
    public interface IRomanConverter
    {
        Tuple<string, int> Convert(int arabicValue);
        Tuple<string, int> Convert(string romanValue);
    }
}
