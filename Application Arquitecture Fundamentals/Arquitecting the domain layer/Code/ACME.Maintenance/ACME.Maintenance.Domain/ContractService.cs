using ACME.Maintenance.Domain.Interfaces;
using ACME.Maintenance.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Maintenance.Domain
{
    public class ContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public Contract GetById(string contractId)
        {
            //var contractRepository = new ContractRepository();
            var contractDTO = _contractRepository.GetById(contractId);

            //var contract = new Contract();
            //contract.ContractId = contractDTO.ContractId;
            //contract.ExpirationDate = contractDTO.ExpirationDate;
            var contract = AutoMapper.Mapper.Map<ContractDTO, Contract>(contractDTO);
            return contract;
        }
    }
}
