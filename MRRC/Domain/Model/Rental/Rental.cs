namespace MRRC
{
    /// <summary>
    /// Rental entity
    /// Lewis Watson 2020
    /// </summary>
    public class Rental : Entity
    {
        public string vehicleRego { get; }
        public int customerID { get; }

        public Rental(string vehicleRego, int customerID)
        {
            this.vehicleRego = vehicleRego;
            this.customerID = customerID;
        }
    }
}
