using System;

namespace MRRC
{
    /// <summary>
    /// Exception for customers when looking up if they exist
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerDoesNotExistException : Exception
    {
        public CustomerDoesNotExistException(string message) : base(message)
        {
        }
    }
}
