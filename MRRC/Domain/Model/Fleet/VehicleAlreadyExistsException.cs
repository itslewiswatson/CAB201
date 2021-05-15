using System;

namespace MRRC
{
    /// <summary>
    /// Custom exception specifically for vehicles in fleet
    /// Lewis Watson 2020
    /// </summary>
    public class VehicleAlreadyExistsException : Exception
    {
        public VehicleAlreadyExistsException(string message) : base(message) { }
    }
}
