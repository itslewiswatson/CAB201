using System;

namespace MRRC
{
    /// <summary>
    /// Custom exception for trying to rent a vehicle that is already being rented
    /// Lewis Watson 2020
    /// </summary>
    public class VehicleCurrentlyRentingException : Exception
    {
        public VehicleCurrentlyRentingException(string message) : base(message) { }
    }
}
