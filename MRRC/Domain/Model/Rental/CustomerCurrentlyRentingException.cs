using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRC
{
    /// <summary>
    /// Custom exception for trying to rent to a customer already renting
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerCurrentlyRentingException : Exception
    {
        public CustomerCurrentlyRentingException(string message) : base(message) { }
    }
}
