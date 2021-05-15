using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Implement functionality required to generate a fleet table
    /// Lewis Watson 2020
    /// </summary>
    public class FleetTable : DynamicTable<Fleet, VehicleEntityParser>
    {
        public FleetTable(Menu parentMenu, FleetRepository fleetRepository) : base(parentMenu, fleetRepository) { }

        /// <summary>
        /// Set up columns for this specific type of table
        /// </summary>
        /// <returns>Dictionary of column name and expected type</returns>
        protected override Dictionary<string, Type> GetColumnTypes()
        {
            Dictionary<string, Type> fleetTypes = new Dictionary<string, Type>();

            fleetTypes.Add("Registration", typeof(string));
            fleetTypes.Add("Grade", typeof(string));
            fleetTypes.Add("Make", typeof(string));
            fleetTypes.Add("Model", typeof(string));
            fleetTypes.Add("Year", typeof(string));
            fleetTypes.Add("Num Seats", typeof(string));
            fleetTypes.Add("Transmission", typeof(string));
            fleetTypes.Add("Fuel", typeof(string));
            fleetTypes.Add("GPS", typeof(string));
            fleetTypes.Add("Sunroof", typeof(string));
            fleetTypes.Add("Daily Rate", typeof(string));
            fleetTypes.Add("Colour", typeof(string));

            return fleetTypes;
        }

        /// <summary>
        /// Get the name of the table
        /// </summary>
        /// <returns>String to be used in parent classes</returns>
        protected override string GetTableName()
        {
            return "Fleet";
        }

        /// <summary>
        /// Predefined width of table
        /// </summary>
        protected override int GetTableWidth()
        {
            return 181;
        }

        /// <summary>
        /// Implementation of updating a table with data
        /// </summary>
        protected override void Update()
        {
            Fleet fleet = repository.Get();
            foreach (Vehicle vehicle in fleet.vehicles)
            {
                AddRow(vehicle.ToStringArray());
            }
        }
    }
}
