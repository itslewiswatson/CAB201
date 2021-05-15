namespace MRRC
{
    /// <summary>
    /// Vehicle data-transfer-object
    /// Lewis Watson 2020
    /// </summary>
    class VehicleDTO : EntityDTO
    {
        public string vehicleRego { get; }
        public string vehicleGrade { get; }
        public string make { get; }
        public string model { get; }
        public string year { get; }
        public string numSeats { get; }
        public string transmission { get; }
        public string fuel { get;}
        public string GPS { get; }
        public string sunRoof { get; }
        public string dailyRate { get; }
        public string colour { get; }
        
        public VehicleDTO(string vehicleRego, string vehicleGrade, string make, string model, string year, string numSeats, string transmission, 
            string fuel, string GPS, string sunRoof, string dailyRate, string colour)
        {
            this.vehicleRego = vehicleRego;
            this.vehicleGrade = vehicleGrade;
            this.make = make;
            this.model = model;
            this.year = year;
            this.numSeats = numSeats;
            this.transmission = transmission;
            this.fuel = fuel;
            this.GPS = GPS;
            this.sunRoof = sunRoof;
            this.dailyRate = dailyRate;
            this.colour = colour;
        }
    }
}
