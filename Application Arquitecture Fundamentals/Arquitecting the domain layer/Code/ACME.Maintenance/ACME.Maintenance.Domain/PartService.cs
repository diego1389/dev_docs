using ACME.Maintenance.Domain.DTO;
using ACME.Maintenance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Maintenance.Domain
{
    public class PartService
    {
        private readonly IPartServiceRepository _partServiceRepository;

        public PartService(IPartServiceRepository partServiceRepository)
        {
            _partServiceRepository = partServiceRepository;
        }

        public Part GetById(string partId)
        {
            var partDto = _partServiceRepository.GetById(partId);
            //do the mapping
            var part = AutoMapper.Mapper.Map<PartDTO, Part>(partDto);
            return part;
        
        }
        
    }
}
