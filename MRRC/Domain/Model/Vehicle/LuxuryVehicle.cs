namespace MRRC
{
    public class LuxuryVehicle : Vehicle
    {
        public new const bool Default_GPS = true;
        public new const bool Default_Sun_Roof = true;
        public new const double Default_Daily_Rate = 120;

        public LuxuryVehicle(
            string vehicleRego,
            string make,
            string model,
            int year,
            int numSeats = Default_Num_Seats,
            TransmissionType transmission = Default_Transmission_Type,
            FuelType fuel = Default_Fuel_Type,
            bool GPS = Default_GPS,
            bool sunRoof = Default_Sun_Roof,
            double dailyRate = Default_Daily_Rate,
            string colour = Default_Colour
        ) : base(
            vehicleRego,
            VehicleGrade.Luxury,
            make,
            model,
            year,
            numSeats,
            transmission,
            fuel,
            GPS,
            sunRoof,
            dailyRate,
            colour
        )
        { }
    }
}
