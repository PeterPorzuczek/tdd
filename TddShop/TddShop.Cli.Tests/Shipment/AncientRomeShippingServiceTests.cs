using Moq;
using NUnit.Framework;
using TddShop.Cli.Shipment;
using TddShop.Cli.Order.Models;
using System.Linq;

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
        public void ShipOrder_OrderIsEmpty_ShouldNeverGenerateReferenceNumber(){
            //Arrange
            var order = new OrderModel(){ CustomerUsername = "CustomerTestowy", Items = new ItemModel[] { } };
            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(order.Items.Sum(p => p.Quantity)), Times.Never());
        }

        [Test]
        public void ShipOrder_OrderIsNotEmpty_ShouldGenerateReferenceOnce()
        {
            //Arrange
            var order = new OrderModel() { 
                CustomerUsername = "CustomerTestowy", 
                Items = new ItemModel[] { 
                    new ItemModel{ Quantity = 1 }, 
                    new ItemModel{ Quantity = 1 } 
                } 
            };

            int orderItemsQuantity = order.Items.Sum(p => p.Quantity);

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(orderItemsQuantity), Times.Once());
        }

        [Test]
        public void ShipOrder_OrderHasNoCustomerName_ShouldNeverGenerateReference()
        {
            //Arrange
            var order = new OrderModel() {
                CustomerUsername = string.Empty,
                Items = new ItemModel[] {
                    new ItemModel{ Quantity = 1 },
                    new ItemModel{ Quantity = 1 }
                }
            };

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.GenerateShipmentReferenceNumber(order.Items.Sum(p => p.Quantity)), Times.Never());
        }

        [Test]
        public void ShipOrder_OrderIsValid_ShouldRequestDeliveryOnce()
        {
            //Arrange
            var order = new OrderModel() {
                CustomerUsername = "CustomerTestowy",
                Items = new ItemModel[] {
                    new ItemModel{ Quantity = 1 },
                    new ItemModel{ Quantity = 1 }
                }
            };

            int orderItemsQuantity = order.Items.Sum(p => p.Quantity);
            string shipmentRefNumberRoman = "II";

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(orderItemsQuantity)).Returns(2);
            _romanConvereter.Setup(x => x.Convert(orderItemsQuantity)).Returns(shipmentRefNumberRoman);

            //Act
            _target.ShipOrder(order);

            //Assert
            _deliveryService.Verify(x => x.RequestDelivery(shipmentRefNumberRoman, order), Times.Once());
        }

        [Test]
        public void ShipOrder_OrderItemIsNull_ExceptionShouldBeThrown() {
            
            //Arrange            
            var order = new OrderModel() {
                CustomerUsername = "CustomerTestowy",
                Items = new ItemModel[] { null }
            };

            //Act
            TestDelegate exceptionDelegate = () => { _target.ShipOrder(order); };

            //Assert
            Assert.Throws(typeof(System.NullReferenceException), exceptionDelegate);
        }

    }
}
