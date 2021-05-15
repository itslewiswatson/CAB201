using System;
using System.Collections.Generic;
using System.Reflection;
using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Handler for editing a vehicle from user input
    /// Lewis Watson 2020
    /// </summary>
    public class EditVehicleHandler : VehicleHandler
    {
        private const int Index_To_Begin = 1;
        private const int Index_To_Remove = Index_To_Begin - 1;

        public EditVehicleHandler(FleetRepository repository) : base(repository) { }

        /// <summary>
        /// The actual execution of editing a vehicle.
        /// Resolve and check for business logic constraints before equalising values and letting the user know it was successful.
        /// </summary>
        /// <param name="args">List of user-defined inputs</param>
        protected override void Execute(List<string> args)
        {
            PropertyInfo[] vehicleProperties = typeof(Vehicle).GetProperties();

            Vehicle existingVehicle = ResolveVehicle(args);
            List<string> trimmedArgs = TrimArguments(args);
            DefaultArguments(ref trimmedArgs, vehicleProperties, existingVehicle);

            Vehicle validatedVehicle = CreateNewVehicle(trimmedArgs);

            // Ensure vehicle isn't being rented
            Fleet fleet = repository.Get();
            Vehicle vehicle = fleet.GetVehicle(validatedVehicle.vehicleRego);
            if (fleet.IsVehicleRented(vehicle.vehicleRego))
            {
                throw new VehicleCurrentlyRentingException(string.Format("{0} is currently being rented and cannot be edited.",
                                                            vehicle.vehicleRego));
            }

            ValidateConstraints(existingVehicle, validatedVehicle);
            EqualiseVehicles(existingVehicle, validatedVehicle);

            Console.WriteLine("Successfully edited vehicle {0} {1} {2} (Registration = {3}).", validatedVehicle.year, validatedVehicle.make, 
                validatedVehicle.model, validatedVehicle.vehicleRego);
            Console.WriteLine();
        }

        /// <summary>
        /// Hard resolve vehicle from fleet, with an exception throwing if it cannot be resolveds
        /// </summary>
        /// <param name="args">List of user-defined inputs</param>
        /// <returns>Vehicle if can be resolved from arguments</returns>
        private Vehicle ResolveVehicle(List<string> args)
        {
            Fleet fleet = repository.Get();

            string vehicleRego = args[Index_To_Remove];
            return fleet.GetVehicle(vehicleRego);
        }

        /// <summary>
        /// Creates new entity from passed arguments
        /// </summary>
        /// <param name="args">List of user-defined inputs</param>
        /// <returns>Newly created unpersisted vehicle</returns>
        private Vehicle CreateNewVehicle(List<string> args)
        {
            FleetRepository fleetRepository = (FleetRepository)repository;

            return (Vehicle)fleetRepository.vehicleEntityParser.CreateEntity(args.ToArray());
        }

        /// <summary>
        /// Remove arguments from user-input that are not relevant to the current entity
        /// </summary>
        /// <param name="args">Parsed user-input arguments</param>
        /// <returns>List of relevant arguments</returns>
        private List<string> TrimArguments(List<string> args)
        {
            return args.GetRange(Index_To_Begin, args.Count - Index_To_Begin); // Remove first element (current registration)
        }

        /// <summary>
        /// Default any blank arguments specified by user to the current vehicle's value
        /// </summary>
        /// <param name="args">List of user-inputted arguments</param>
        /// <param name="vehicleProperties">List of reflection-based vehicle properties</param>
        /// <param name="existingVehicle">Vehicle to pull values from</param>
        private void DefaultArguments(ref List<string> args, PropertyInfo[] vehicleProperties, Vehicle existingVehicle)
        {
            for (int index = 0; index < args.Count; index++)
            {
                string value = args[index].ToString().Trim(); // Value as entered by user
                // Use existing value if input was blank
                if (value == "")
                {
                    // Use reflection to overwrite user-entered input
                    var currentValue = vehicleProperties[index].GetValue(existingVehicle).ToString();
                    args[index] = currentValue;
                }
            }
        }

        /// <summary>
        /// Validate business logic surrounding vehicles
        /// </summary>
        /// <param name="existingVehicle">Vehicle to update parameters on</param>
        /// <param name="validatedVehicle">Vehicle to update parameters from</param>
        private void ValidateConstraints(Vehicle existingVehicle, Vehicle validatedVehicle)
        {
            Fleet fleet = repository.Get();

            if (validatedVehicle.vehicleRego != existingVehicle.vehicleRego)
            {
                Vehicle newRegoVehicle = fleet.FindVehicle(validatedVehicle.vehicleRego);
                if (newRegoVehicle != null)
                {
                    throw new VehicleAlreadyExistsException(string.Format("Vehicle with registration {0} already exists. Please choose a " +
                                                            "different registration.", validatedVehicle.vehicleRego));
                }
            }
        }

        /// <summary>
        /// Equalise the properties of two vehicle entities
        /// </summary>
        /// <param name="existingVehicle">Vehicle to update parameters on</param>
        /// <param name="validatedVehicle">Vehicle to update parameters from</param>
        private void EqualiseVehicles(Vehicle existingVehicle, Vehicle validatedVehicle)
        {
            existingVehicle.vehicleRego = validatedVehicle.vehicleRego;
            existingVehicle.vehicleGrade = validatedVehicle.vehicleGrade;
            existingVehicle.make = validatedVehicle.make;
            existingVehicle.model = validatedVehicle.model;
            existingVehicle.year = validatedVehicle.year;
            existingVehicle.numSeats = validatedVehicle.numSeats;
            existingVehicle.transmission = validatedVehicle.transmission;
            existingVehicle.fuel = validatedVehicle.fuel;
            existingVehicle.GPS = validatedVehicle.GPS;
            existingVehicle.sunRoof = validatedVehicle.sunRoof;
            existingVehicle.dailyRate = validatedVehicle.dailyRate;
            existingVehicle.colour = validatedVehicle.colour;
        }
    }
}
