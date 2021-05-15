using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    /// Vehicle entity container
    /// Lewis Watson 2020
    /// </summary>
    abstract public class Vehicle : Entity
    {
        public const int Min_Num_Seats = 2;
        public const int Max_Num_Seats = 10;

        public const int Default_Num_Seats = 4;
        public const TransmissionType Default_Transmission_Type = TransmissionType.Manual;
        public const FuelType Default_Fuel_Type = FuelType.Petrol;
        public const bool Default_GPS = false;
        public const bool Default_Sun_Roof = false;
        public const double Default_Daily_Rate = 50;
        public const string Default_Colour = "Black";

        public string vehicleRego { get; set; }
        public VehicleGrade vehicleGrade { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public int numSeats { get; set; } = Default_Num_Seats;
        public TransmissionType transmission { get; set; } = Default_Transmission_Type;
        public FuelType fuel { get; set; } = Default_Fuel_Type;
        public bool GPS { get; set; } = Default_GPS;
        public bool sunRoof { get; set; } = Default_Sun_Roof;
        public double dailyRate { get; set; } = Default_Daily_Rate;
        public string colour { get; set; } = Default_Colour;

        public Vehicle(string vehicleRego, VehicleGrade vehicleGrade, string make, string model, int year, int numSeats, 
                        TransmissionType transmission, FuelType fuel, bool GPS, bool sunRoof, double dailyRate, string colour)
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

        public List<string> GetAttributeList()
        {
            List<string> attributes = new List<string>();

            attributes.Add(vehicleRego);
            attributes.Add(vehicleGrade.ToString());
            attributes.Add(make);
            attributes.Add(model);
            attributes.Add(year.ToString());
            attributes.Add(string.Format("{0}-Seater", numSeats.ToString()));
            attributes.Add(transmission.ToString());
            attributes.Add(fuel.ToString());
            if (GPS)
            {
                attributes.Add("GPS");
            }
            if (sunRoof)
            {
                attributes.Add("Sunroof");
            }
            attributes.Add(colour);

            return attributes;
        }
    }
}