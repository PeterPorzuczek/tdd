using Moq;
using NUnit.Framework;
using TddShop.Cli.Shipment;
using TddShop.Cli.Order.Models;

namespace TddShop.Cli.Tests.Shipment
{
    [TestFixture]
    public class AncientRomeShippingServiceTests
    {
        private Mock<IDeliveryService> _deliveryService;
        private Mock<IRomanConverter> _romanConvereter;
        private AncientRomeShippingService _target;

        [SetUp]
        public void Initialize()
        {
            _deliveryService = new Mock<IDeliveryService>();
            _romanConvereter = new Mock<IRomanConverter>();

            _target = new AncientRomeShippingService(_deliveryService.Object, _romanConvereter.Object);
        }


        [Test]
        public void ShipOrder_OrderEmpty_ShouldGenerateReferenceNever(){
            //Arrange
            var order = new OrderModel();
            order.CustomerUsername = string.Empty;
            order.Items = new ItemModel[] { };
            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(order.Items.Length), Times.Never());
        }

        [Test]
        public void ShipOrder_OrderNotEmpty_ShouldGenerateReferenceOnce()
        {
            //Arrange
            var order = new OrderModel();
            order.CustomerUsername = "CustomerTestowy";
            order.Items = new ItemModel[] {
                new ItemModel{ },
                new ItemModel{ }
            };

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(order.Items.Length), Times.Once());
        }

        [Test]
        public void ShipOrder_OrderHasNoCustomer_ShouldGenerateReferenceNever()
        {
            //Arrange
            var order = new OrderModel();
            order.CustomerUsername = string.Empty;
            order.Items = new ItemModel[] {
                new ItemModel{ },
                new ItemModel{ }
            };

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(order.Items.Length), Times.Never());
        }

        [Test]
        public void ShipOrder_ReferenceNumberValid_ShouldConvertOnce()
        {
            //Arrange
            var order = new OrderModel();
            order.CustomerUsername = "CustomerTestowy";
            order.Items = new ItemModel[] { new ItemModel { }, new ItemModel { } };

            int shipmentRefNumberArabic = order.Items.Length;

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(order.Items.Length)).Returns(shipmentRefNumberArabic);

            //Act
            _target.ShipOrder(order);

            //Assert
            _romanConvereter.Verify(x => x.Convert(shipmentRefNumberArabic), Times.Once());
        }

        [Test]
        public void ShipOrder_OrderValid_ShouldDeliveryRequestOnce()
        {
            //Arrange
            var order = new OrderModel();
            order.CustomerUsername = "CustomerTestowy";
            order.Items = new ItemModel[] { new ItemModel { }, new ItemModel { } };

            string shipmentRefNumberRoman = "II";

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(order.Items.Length)).Returns(2);
            _romanConvereter.Setup(x => x.Convert(order.Items.Length)).Returns(shipmentRefNumberRoman);

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.RequestDelivery(shipmentRefNumberRoman, order), Times.Once());
        }

    }
}
