using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    /// Concrete implementation of the fleet repository
    /// Lewis Watson 2020
    /// </summary>
    public class FleetRepository : Repository<Fleet, VehicleEntityParser>
    {
        public VehicleEntityParser entityParser { get; }

        public VehicleEntityParser vehicleEntityParser { get; }
        public RentalEntityParser rentalEntityParser { get; }

        private Fleet fleet { get; set; }

        public FleetRepository(VehicleEntityParser vehicleEntityParser, RentalEntityParser rentalEntityParser)
        {
            this.vehicleEntityParser = vehicleEntityParser;
            this.rentalEntityParser = rentalEntityParser;
        }

        /// <summary>
        /// Return or create an isntance of a fleet, including loading assocated entities
        /// </summary>
        public Fleet Get()
        {
            if (fleet == null)
            {
                fleet = new Fleet(SelectAllVehicles(), SelectAllRentals());
            }
            return fleet;
        }

        /// <summary>
        /// Load all vehicles from disk
        /// </summary>
        /// <returns>List of vehicle entities</returns>
        public List<Vehicle> SelectAllVehicles()
        {
            return vehicleEntityParser.LoadAll();
        }

        /// <summary>
        /// Load all rentals from disk
        /// </summary>
        /// <returns>List of rental entities</returns>
        public List<Rental> SelectAllRentals()
        {
            return rentalEntityParser.LoadAll();
        }

        /// <summary>
        /// Write all fleet data to disk
        /// </summary>
        public void Flush()
        {
            vehicleEntityParser.SaveAll(fleet.vehicles);
            rentalEntityParser.SaveAll(fleet.rentals);
        }
    }
}
