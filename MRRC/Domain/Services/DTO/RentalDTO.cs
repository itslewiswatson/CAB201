namespace MRRC
{
    /// <summary>
    /// Rental data-transfer-object
    /// Lewis Watson 2020
    /// </summary>
    public class RentalDTO : EntityDTO
    {
        public string vehicleRego { get; }
        public string customerID { get; }

        public RentalDTO(string vehicleRego, string customerID)
        {
            this.vehicleRego = vehicleRego;
            this.customerID = customerID;
        }
    }
}
