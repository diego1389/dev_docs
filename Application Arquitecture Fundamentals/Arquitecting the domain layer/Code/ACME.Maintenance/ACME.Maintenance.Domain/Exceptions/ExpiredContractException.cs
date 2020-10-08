using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Maintenance.Domain.Exceptions
{
    public class ExpiredContractException : Exception, ISerializable
    {
        public ExpiredContractException() : base()
        {
                
        }
        public ExpiredContractException(string message) : base(message)
        {

        }

        public ExpiredContractException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExpiredContractException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
