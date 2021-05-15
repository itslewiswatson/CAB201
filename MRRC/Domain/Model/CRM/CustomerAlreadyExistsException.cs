using System;

namespace MRRC
{
    /// <summary>
    /// Custom customer exception for handling customers that already exist when adding new customers or editing existing customers and
    /// assgining them a new ID
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
