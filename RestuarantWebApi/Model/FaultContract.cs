using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestuarantWebApi.Model
{
    public class FaultContract
    {
        public string faultmessage { get; set; }

        public EfaultCode faultcode { get; set; }

        /// <summary>
        /// Constructor of Fault Contract.
        /// </summary>
        public FaultContract()
        {
                     
        }

    }
}
