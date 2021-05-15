using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Handler for returning a vehicle from user input
    /// Lewis Watson 2020
    /// </summary>
    public class ReturnVehicleHandler : RentalHandler
    {
        public ReturnVehicleHandler(FleetRepository repository) : base(repository) { }

        /// <summary>
        /// Return a vehicle from user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            // Setup variables
            Fleet fleet = repository.Get();
            string vehicleRego = args[0];

            fleet.GetVehicle(vehicleRego); // Call this to verify vehicle exists (will throw exception)

            // Disallow returning vehicles not being rented
            if (!fleet.IsVehicleRented(vehicleRego))
            {
                throw new Exception(string.Format("Vehicle {0} is not currently being rented. No need to return it.", vehicleRego));
            }

            fleet.ReturnVehicle(vehicleRego);

            // Let user know that it has been successful
            Console.WriteLine(string.Format("Successfully returned vehicle {0} to the fleet. It can now be rented to other customers.", 
                                vehicleRego));
            Console.WriteLine();
        }
    }
}
