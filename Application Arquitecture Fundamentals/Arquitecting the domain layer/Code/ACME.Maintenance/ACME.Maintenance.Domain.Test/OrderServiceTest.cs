using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACME.Maintenance.Domain.Interfaces;
using FakeItEasy;
using ACME.Maintenance.Domain.Exceptions;
using ACME.Maintenance.Domain.DTO;
using ACME.Maintenance.Domain;
using AutoMapper;
using System.Collections.Generic;
using ACME.Maintenance.Domain.Enums;

namespace ACME.Maintenance.Domain.Test
{
    [TestClass]
    public class OrderServiceTest
    {
        private IContractRepository _contractRepository;
        private ContractService _contractService;
        private IPartServiceRepository _partServiceRepository;

        private const string ValidContractId = "CONTRACTID";
        private const string ExpiredContractId = "EXPIREDCONTRACTID";
        private const string ValidPartId = "VALIDPARTID";
        private const double ValidPartPrice = 50.0;

        [TestInitialize]
        public void Initialize()
        {
            _contractRepository = A.Fake<IContractRepository>();
            _contractService = new ContractService(_contractRepository);
            _partServiceRepository = A.Fake<IPartServiceRepository>();

            A.CallTo(() => _contractRepository.GetById(ValidContractId)).Returns(new ContractDTO
            {
                ContractId = ValidContractId,
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            A.CallTo(() => _contractRepository.GetById(ExpiredContractId)).Returns(new ContractDTO
            {
                ContractId = ExpiredContractId,
                ExpirationDate = DateTime.Now.AddDays(-1)
            });


            A.CallTo(() => _partServiceRepository.GetById(ValidPartId)).Returns(new PartDTO {
                PartId = ValidPartId,
                Price = ValidPartPrice
            });
            Mapper.Initialize(cfg =>
            { 
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<PartDTO, Part>();
            }
            );
        }

        [TestMethod]
        public void CreateOrder_ValidContract_CreateNewOrder()
        {
            //Arrange
            var orderService = new OrderService();
            var contractService = new ContractService(_contractRepository);
            var contract = contractService.GetById(ValidContractId);
            //Act
            var newOrder = orderService.CreateOrder(contract);
            //Assert

            Assert.IsInstanceOfType(newOrder, typeof(Order));
            Guid guidOut;
            Assert.IsTrue(Guid.TryParse(newOrder.OrderId.ToString(), out guidOut));
            //Assert.AreEqual(newOrder.OrderId, OrderId);
            Assert.AreEqual(newOrder.Status, OrderStatus.New);
            Assert.IsInstanceOfType(newOrder.Items, typeof(IReadOnlyList<OrderItem>));
        }

        [TestMethod, ExpectedException(typeof(ExpiredContractException))]
        public void CreateOrder_ExpiredContract_ThrowsException()
        {
            //Arrange
            var orderService = new OrderService();
            var contractService = new ContractService(_contractRepository);
            var contract = contractService.GetById(ExpiredContractId);

            //Act
            var newOrder = orderService.CreateOrder(contract);

        }
       
        [TestMethod]
         public void CreateOrderItem_ValidPart_CreatesOrderItem()
         {
             //Arrange
             var orderService = new OrderService();
             var contractService = new ContractService(_contractRepository);
             var contract = contractService.GetById(ValidContractId);

             var order = orderService.CreateOrder(contract);
             var partService = new PartService(_partServiceRepository);
             var part = partService.GetById(ValidPartId);
            var quantity = 1;

             var orderItem = orderService.CreateOrderItem(part, quantity);

            //Assert
            Assert.AreEqual(orderItem.Part, part);
            Assert.AreEqual(orderItem.Quantity, quantity);
            Assert.AreEqual(orderItem.Price, ValidPartPrice);
            Assert.AreEqual(orderItem.LineTotal, ValidPartPrice * quantity);
         }
         

    }
}
