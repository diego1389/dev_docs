using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Maintenance.Domain.Exceptions
{
    public class ContractNotFoundException : Exception, ISerializable
    {
        public ContractNotFoundException() : base()
        {
                
        }

        public ContractNotFoundException(string message) : base(message)
        {

        }

        public ContractNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }

        public ContractNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

    }
}
