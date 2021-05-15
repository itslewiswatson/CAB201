using MRRC;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Implement functionality required to generate a rental table with access to fleet
    /// Lewis Watson 2020
    /// </summary>
    public class SearchResultTable : FleetTable
    {
        private List<Vehicle> results { get; set; } = new List<Vehicle>();

        public SearchResultTable(Menu parentMenu, FleetRepository fleetRepository, List<Vehicle> results) : base(parentMenu, fleetRepository)
        {
            this.results = results;
        }

        /// <summary>
        /// Implementation of updating a table with data
        /// </summary>
        protected override void Update()
        {
            foreach (Vehicle vehicle in results)
            {
                AddRow(vehicle.ToStringArray());
            }
        }
    }
}
