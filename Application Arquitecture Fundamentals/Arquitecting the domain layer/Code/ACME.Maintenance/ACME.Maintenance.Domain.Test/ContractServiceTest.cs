using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACME.Maintenance.Domain.Interfaces;
using ACME.Maintenance.Domain.DTO;
using ACME.Maintenance.Domain.Exceptions;
using FakeItEasy;
using AutoMapper;


namespace ACME.Maintenance.Domain.Test
{
    [TestClass]
    public class ContractServiceTest
    {
        private IContractRepository _contractRepository;
        private ContractService _contractService;

        private const string ValidContractId = "CONTRACTID";
        private const string ExpiredContractId = "EXPIREDCONTRACTID";
        private const string InvalidContractId = "INVALIDCONTRACTID";

        [TestInitialize]
        public void Initialize()
        {
            _contractRepository = A.Fake<IContractRepository>();
            _contractService = new ContractService(_contractRepository);

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

            A.CallTo(() => _contractRepository.GetById(InvalidContractId))
                .Throws<ContractNotFoundException>();

            Mapper.Initialize(cfg => cfg.CreateMap<ContractDTO, Contract>());
        }

        [TestMethod]
        public void GetById_ValidContractId_ReturnsContract()
        {
            //Act
            var contract = _contractService.GetById(ValidContractId);
            //Assert
            Assert.IsInstanceOfType(contract, typeof(Contract));
            Assert.IsTrue(contract.ExpirationDate > DateTime.Now);
            Assert.AreEqual(ValidContractId, contract.ContractId);
        }

        [TestMethod]
        public void GetById_ExpiredContractId_ReturnsExpiredContract()
        {
            var contract = _contractService.GetById(ExpiredContractId);
            Assert.IsInstanceOfType(contract, typeof(Contract));
            Assert.IsTrue(contract.ExpirationDate < DateTime.Now);
            Assert.AreEqual(ExpiredContractId, contract.ContractId);
        }

        [TestMethod, ExpectedException(typeof(ContractNotFoundException))]
        public void GetById_InvalidContractId_ThrowsException()
        {
            var contract = _contractService.GetById(InvalidContractId);
        }



    }
}
