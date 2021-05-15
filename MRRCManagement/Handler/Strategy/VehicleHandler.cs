using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Implement the handler for a vehicle for easier type-hinting
    /// Lewis Watson 2020
    /// </summary>
    abstract public class VehicleHandler : Handler<Fleet, VehicleEntityParser>
    {
        /// <summary>
        /// Pass the repository to the parent with improved type-hinting
        /// </summary>
        /// <param name="repository">Repository that corresponds with the expected repository for the entity</param>
        public VehicleHandler(FleetRepository repository) : base(repository) { }
    }
}
