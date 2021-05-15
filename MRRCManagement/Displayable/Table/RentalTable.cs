using MRRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    /// <summary>
    /// Implement functionality required to generate a rental table
    /// Lewis Watson 2020
    /// </summary>
    class RentalTable : DynamicTable<Fleet, VehicleEntityParser>
    {
        public RentalTable(Menu parentMenu, FleetRepository fleetRepository) : base(parentMenu, fleetRepository) { }

        protected override Dictionary<string, Type> GetColumnTypes()
        {
            Dictionary<string, Type> fleetTypes = new Dictionary<string, Type>();

            fleetTypes.Add("Registration", typeof(string));
            fleetTypes.Add("Customer ID", typeof(string));
            fleetTypes.Add("Daily Rate", typeof(string));

            return fleetTypes;
        }

        /// <summary>
        /// Get the name of the table
        /// </summary>
        /// <returns>String to be used in parent classes</returns>
        protected override string GetTableName()
        {
            return "Rentals";
        }

        /// <summary>
        /// Predefined width of table
        /// </summary>
        protected override int GetTableWidth()
        {
            return 50;
        }

        /// <summary>
        /// Implementation of updating a table with data
        /// </summary>
        protected override void Update()
        {
            Fleet fleet = repository.Get();

            foreach (Rental rental in fleet.rentals)
            {
                List<string> row = rental.ToStringArray().ToList();

                Vehicle vehicle = fleet.FindVehicle(rental.vehicleRego);
                if (vehicle != null)
                {
                    string dailyRate = vehicle.dailyRate.ToString();
                    row.Add(dailyRate);
                }
                // For unlikely case of a rental that does not exist in fleet
                else
                {
                    row.Add("n/a");
                }

                AddRow(row.ToArray());
            }
        }
    }
}
