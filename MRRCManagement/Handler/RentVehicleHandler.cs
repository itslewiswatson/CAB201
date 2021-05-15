using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Handler for renting a vehicle from user input
    /// Lewis Watson 2020
    /// </summary>
    public class RentVehicleHandler : RentalHandler
    {
        private CRMRepository crmRepository { get; }

        public RentVehicleHandler(FleetRepository repository, CRMRepository crmRepository) : base(repository)
        {
            this.crmRepository = crmRepository;
        }

        /// <summary>
        /// Rents a vehicle based on user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            // Setup variables
            int customerID = int.Parse(args[0]);
            string vehicleRego = args[1];
            int days = int.Parse(args[2]);
            FleetRepository fleetRepository = (FleetRepository)repository;
            Fleet fleet = fleetRepository.Get();
            CRM crm = crmRepository.Get();

            // Resolve 
            Vehicle vehicle = fleet.GetVehicle(vehicleRego);
            Customer customer = crm.GetCustomer(customerID);

            // Inform user of cost with lots of details
            Console.WriteLine("Rent {0} to {1} {2} (ID: {3}) for {4} days.", vehicle.vehicleRego, customer.firstName, customer.lastName, customer.ID,
                                days);
            Console.WriteLine("This vehicle has a daily cost of ${0}, bringing the total to ${1:0.00}.", vehicle.dailyRate, 
                                (vehicle.dailyRate * days));
            Console.WriteLine();

            // Make user confirm rental at price
            string input = "";
            do {
                Console.Write("Confirm you want to go ahead with this rental (y/n): ");
                input = Console.ReadLine().Trim().ToLower();
                if (input == "y")
                {
                    break;
                }
                if (input == "n")
                {
                    return;
                }
            } while (input != "y" || input != "n");

            // Let user know it was successful
            Console.WriteLine();
            fleet.RentVehicle(vehicle.vehicleRego, customer.ID);
            Console.WriteLine("Successfully rented a {0} {1} {2} to {3} {4} (ID: {5})", vehicle.year, vehicle.make, vehicle.model, 
                                customer.firstName, customer.lastName, customer.ID);
            Console.WriteLine();
        }
    }
}
