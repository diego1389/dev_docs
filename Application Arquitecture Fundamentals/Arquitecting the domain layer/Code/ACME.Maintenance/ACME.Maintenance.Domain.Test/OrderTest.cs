using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACME.Maintenance.Domain.Test
{
    [TestClass]
    public class OrderTest
    {
        private OrderService _orderService = new OrderService();
        private Order _order;
        private Contract _contract;
        private Part _part = new Part()
        {
            PartId = "PARTID",
            Price = 50.0
        };

        [TestInitialize]
        public void Initialize()
        {
            _contract = new Contract()
            {
                ContractId = "VALIDCONTRACTID",
                ExpirationDate = DateTime.Now
            };
            _order = _orderService.CreateOrder(_contract);   
        }


        [TestMethod]
        public void AddOrderItem_ValidOrderItem_AddsOrderItem()
        {
            
            var orderItem = new OrderItem()
            {
                Part = _part,
                Price = _part.Price,
                Quantity = 1,
                LineTotal = _part.Price * 1
            };

            _order.AddOrderItem(orderItem);

            Assert.AreEqual(_order.OrderItemCount, 1);
            Assert.AreEqual(_order.SubTotal, 50.0);
        }

        [TestMethod]
        public void Items_ContainingValidOrderItems_CanBeIteratedOver()
        {
            var orderItem1 = new OrderItem()
            {
                Part = _part,
                Price = _part.Price,
                Quantity = 1,
                LineTotal = _part.Price * 1
            };
            var orderItem2 = new OrderItem()
            {
                Part = _part,
                Price = _part.Price,
                Quantity = 2,
                LineTotal = _part.Price * 2
            };
            var orderItem3 = new OrderItem()
            {
                Part = _part,
                Price = _part.Price,
                Quantity = 3,
                LineTotal = _part.Price * 3
            };

            _order.AddOrderItem(orderItem1);
            _order.AddOrderItem(orderItem2);
            _order.AddOrderItem(orderItem3);

            //Act
            foreach (var item in _order.Items)
            {
                Assert.IsInstanceOfType(item, typeof(OrderItem));
                Assert.IsInstanceOfType(item.Part, typeof(Part));
                Assert.IsTrue(item.Part.Price > 0);
                Assert.IsTrue(item.Quantity > 0);
                Assert.IsTrue(item.LineTotal > 0);
            }
            Assert.AreEqual(_order.Items.Count, 3);
            Assert.AreEqual(_order.SubTotal, 300.0);
            Assert.AreEqual(_order.OrderItemCount, 6);
        }

        [TestMethod]
        public void Items_ContainingNoOrders_DoesNotThrowException()
        {
            foreach (var item in _order.Items)
            {
                Assert.Fail("There should be any items here.");
            }

            Assert.AreEqual(_order.Items.Count, 0);
            Assert.AreEqual(_order.SubTotal, 0.0);
            Assert.AreEqual(_order.OrderItemCount, 0);
        }
    }
}
