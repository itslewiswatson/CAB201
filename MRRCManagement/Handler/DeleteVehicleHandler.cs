using MRRC;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MRRCManagement
{
    /// <summary>
    /// Handles deletion of vehicles from the fleet through user input
    /// Lewis Watson 2020
    /// </summary>
    class DeleteVehicleHandler : VehicleHandler
    {
        private const int Index_To_Use = 0;

        public DeleteVehicleHandler(FleetRepository repository) : base(repository) { }

        /// <summary>
        /// Removes a vehicle from the fleet using user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            Fleet fleet = repository.Get();

            string vehicleRego = args[Index_To_Use];

            // Disallow deleting vehicles being rented
            Vehicle vehicle = fleet.GetVehicle(vehicleRego);
            if (fleet.IsVehicleRented(vehicle.vehicleRego))
            {
                throw new VehicleCurrentlyRentingException(string.Format("{0} is currently being rented and cannot be deleted.",
                                                            vehicle.vehicleRego));
            }

            fleet.RemoveVehicle(vehicle);

            Console.WriteLine("Successfully removed a {0} {1} {2} with registration {3} from the MRRC fleet.", vehicle.year, vehicle.make, 
                vehicle.model, vehicle.vehicleRego);
            Console.WriteLine();
        }
    }
}
