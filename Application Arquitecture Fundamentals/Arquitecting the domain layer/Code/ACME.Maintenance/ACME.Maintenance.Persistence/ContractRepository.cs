using ACME.Maintenance.Domain;
using ACME.Maintenance.Domain.DTO;
using ACME.Maintenance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Maintenance.Persistence
{
    public class ContractRepository : IContractRepository
    {
        public ContractDTO GetById(string contractId)
        {
            var contractDTO = new ContractDTO();

            //Stubbed for now
            contractDTO.ContractId = contractId;
            contractDTO.ExpirationDate = DateTime.Now.AddDays(1);
            return contractDTO;
        }

    }

}
