using MRRC;
using System.Collections.Generic;
using System;

namespace MRRCManagement
{
    /// <summary>
    /// Handles the addition of new vehicles and their assignment to the fleet from user 
    /// Lewis Watson 2020
    /// </summary>
    class AddVehicleHandler : VehicleHandler
    {
        public AddVehicleHandler(FleetRepository repository) : base(repository) { }

        /// <summary>
        /// Handle high-level execution of adding a vehicle
        /// </summary>
        /// <param name="args">List of user-inputted arguments</param>
        protected override void Execute(List<string> args)
        {
            Fleet fleet = repository.Get();
            Vehicle vehicle = CreateVehicle(args);

            ValidateConstraints(vehicle, fleet);
            AddVehicleToFleet(vehicle, fleet);

            Console.WriteLine("Successfully added a {0} {1} {2} (registration = {3}) to the fleet.", vehicle.year, vehicle.make, vehicle.model, 
                                vehicle.vehicleRego);
            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">List of user-inputted arguments</param>
        /// <returns></returns>
        private Vehicle CreateVehicle(List<string> args)
        {
            return (Vehicle)CreateEntity(args);
        }

        /// <summary>
        /// Adds the vehicle to the fleet
        /// </summary>
        /// <param name="vehicle">Vehicle to add to fleet</param>
        /// <param name="fleet">Fleet to add the vehicle to</param>
        private void AddVehicleToFleet(Vehicle vehicle, Fleet fleet)
        {
            fleet.AddVehicle(vehicle);
        }

        /// <summary>
        /// Validates the newly created vehicle against the rest of the fleet
        /// </summary>
        /// <param name="newVehicle">Vehicle to validate</param>
        /// <param name="fleet">Fleet to validate against</param>
        private void ValidateConstraints(Vehicle newVehicle, Fleet fleet)
        {
            foreach (Vehicle vehicle in fleet.vehicles)
            {
                if (vehicle.vehicleRego.ToLower() == newVehicle.vehicleRego.ToLower())
                {
                    throw new Exception(string.Format("This registration ({0}) is already in use. Please choose another.", vehicle.vehicleRego));
                }
            }
        }

        /// <summary>
        /// Creates the specifed vehicle from user arguments
        /// </summary>
        /// <param name="args">List of user-inputted arguments</param>
        /// <returns></returns>
        private Entity CreateEntity(List<string> args)
        {
            // Generic repository has a M:1 with an entity parser
            // So we must manually cast the generic Repository<T, U> to access its custom entityParser properties
            FleetRepository fleetRepository = (FleetRepository)repository; 
            return fleetRepository.vehicleEntityParser.CreateEntity(args.ToArray());
        }
    }
}
