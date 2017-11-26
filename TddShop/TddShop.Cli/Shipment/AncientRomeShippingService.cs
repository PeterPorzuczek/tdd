using System;
using System.Linq;
using TddShop.Cli.Order.Models;

namespace TddShop.Cli.Shipment
{
    public class AncientRomeShippingService
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IRomanConverter _romanConverter;

        public AncientRomeShippingService(IDeliveryService deliveryService, IRomanConverter romanConverter)
        {
            _deliveryService = deliveryService;
            _romanConverter = romanConverter;
        }

        /// <summary>
        /// To ship an order you need to generate a shipment reference number (see IDeliveryService).
        /// Ancient Rome works with romanian numbers so you will need to convert shipment reference number to a valid romanian number (string).
        /// Use IDeliveryService to ship an order.
        ///                
        /// </summary>
        /// <param name="order"></param>
        public void ShipOrder(OrderModel order)
        {
            if (ValidOrder(order))
            {
                int shipmentRefNumberArabic = _deliveryService.GenerateShipmentReferenceNumber(order.Items.Length);
                string shipmentRefNumberRoman = _romanConverter.Convert(shipmentRefNumberArabic);

                _deliveryService.RequestDelivery(shipmentRefNumberRoman, order);
            }
        }

        private bool ValidOrder(OrderModel order) {
            if (order.Items.Length == 0) {
                return false;
            }
            if (order.Items.Sum(p => p.Quantity) == 0) {
                return false;
            }
            if (string.IsNullOrWhiteSpace(order.CustomerUsername)) {
                return false;
            }
            return true;
        }
    }    
}
