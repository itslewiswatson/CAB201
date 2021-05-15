namespace MRRC
{
    /// <summary>
    /// Concrete implementation of a fleet repository factory
    /// Lewis Watson 2020
    /// </summary>
    public class FleetRepositoryFactory : RepositoryFactoryStrategy<FleetRepository, VehicleEntityParser>
    {
        private string vehicleDirectory { get; }
        private string rentalDirectory { get; }

        public FleetRepositoryFactory(string vehicleDirectory, string rentalDirectory)
        {
            this.vehicleDirectory = vehicleDirectory;
            this.rentalDirectory = rentalDirectory;
        }

        /// <summary>
        /// Parser appropriate for creation of the repo as defined in the generic parameters
        /// </summary>
        /// <returns>Entity parser for use in repository creation</returns>
        public VehicleEntityParser GetEntityParser()
        {
            return new VehicleEntityParser(vehicleDirectory);
        }

        /// <summary>
        /// Second parser appropriate for creation of the repo as not defined in the generic parameters
        /// Special use case
        /// </summary>
        /// <returns>Entity parser for use in repository creation</returns>
        public RentalEntityParser GetRentalEntityParser()
        {
            return new RentalEntityParser(rentalDirectory);
        }

        /// <summary>
        /// Spins up a new instance of the repository
        /// </summary>
        /// <returns>An instance of the defined repository according to the strategy generic parameters</returns>
        public FleetRepository GetRepo()
        {
            return new FleetRepository(GetEntityParser(), GetRentalEntityParser());
        }
    }
}
