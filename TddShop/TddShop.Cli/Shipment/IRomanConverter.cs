using System;
namespace TddShop.Cli.Shipment
{
    public interface IRomanConverter
    {
        string Convert(int arabicValue);
        int Convert(string romanValue);
    }
}
