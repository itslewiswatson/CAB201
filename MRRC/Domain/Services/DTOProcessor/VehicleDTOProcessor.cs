using System;

namespace MRRC
{
    /// <summary>
    /// Conrete implementation of a DTO processor specifically for the vehicle
    /// Lewis Watson 2020
    /// </summary>
    class VehicleDTOProcessor : DTOPrcocessor<Vehicle, VehicleDTO>
    {
        /// <summary>
        /// Converts a VehicleDTO to a Vehicle entity
        /// </summary>
        /// <param name="objectDTO">A data-transfer object with all the same basic properties as a Vehicle</param>
        /// <returns>A fully-fledged vehicle entity that isn't persisted</returns>
        public Vehicle ConvertToEntity(VehicleDTO objectDTO)
        {
            VehicleGrade vehicleGrade = GetVehicleGrade(objectDTO.vehicleGrade);
            int year = int.Parse(objectDTO.year);
            TransmissionType transmission = GetTransmissionType(objectDTO.transmission) ?? GetDefaultTransmissionType(vehicleGrade);
            int numSeats = GetNumSeats(objectDTO.numSeats) ?? Vehicle.Default_Num_Seats;
            FuelType fuel = GetFuelType(objectDTO.fuel) ?? GetDefaultFuelType(vehicleGrade);
            bool GPS = GetGPS(objectDTO.GPS) ?? GetDefaultGPS(vehicleGrade);
            bool sunRoof = GetSunRoof(objectDTO.sunRoof) ?? GetDefaultSunRoof(vehicleGrade);
            double dailyRate = GetDailyRate(objectDTO.dailyRate) ?? GetDefaultDailyRate(vehicleGrade);
            string colour = GetColour(objectDTO.colour) ?? Vehicle.Default_Colour;

            // Create correct type of vehicle based on vehicle grade
            if (vehicleGrade == VehicleGrade.Luxury)
            {
                return new LuxuryVehicle(objectDTO.vehicleRego, objectDTO.make, objectDTO.model, year, numSeats, transmission, fuel, GPS, sunRoof, 
                                            dailyRate, colour);
            }
            if (vehicleGrade == VehicleGrade.Commercial)
            {
                return new CommercialVehicle(objectDTO.vehicleRego, objectDTO.make, objectDTO.model, year, numSeats, transmission, fuel, GPS, 
                                            sunRoof, dailyRate, colour);
            }
            if (vehicleGrade == VehicleGrade.Economy)
            {
                return new EconomyVehicle(objectDTO.vehicleRego, objectDTO.make, objectDTO.model, year, numSeats, transmission, fuel, GPS, sunRoof, 
                                            dailyRate, colour);
            }
            if (vehicleGrade == VehicleGrade.Family)
            {
                return new FamilyVehicle(objectDTO.vehicleRego, objectDTO.make, objectDTO.model, year, numSeats, transmission, fuel, GPS, sunRoof,
                                            dailyRate, colour);
            }

            throw new Exception("Could not convert DTO to correct Vehicle entity variant");
        }

        /// <summary>
        /// Returns the correctly structured enum to determine a vehicle's grade.
        /// Will throw an exception if it cannot resolve the string to the correct enum value.
        /// </summary>
        /// <param name="vehicleGrade">A raw string of the vehicleGrade not in enum form</param>
        /// <returns>Vehicle grade enum value</returns>
        private VehicleGrade GetVehicleGrade(string vehicleGrade)
        {
            try
            {
                vehicleGrade = EnumHelper.MakeFriendlyString(vehicleGrade);
                return (VehicleGrade)Enum.Parse(typeof(VehicleGrade), vehicleGrade);
            }
            catch (Exception)
            {
                throw new Exception("Vehicle grade must be commercial, luxury, family or economy.");
            }
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a TransmissionType for the Vehicle entity.
        /// Will throw an exception if it cannot resolve the string to the correct enum value.
        /// </summary>
        /// <param name="transmission">A raw string of the transmission not in enum form</param>
        /// <returns>Enum that determines the vehicle's transmission, or null to use the existing vehicle's 
        /// transmission value or entity default</returns>
        private TransmissionType? GetTransmissionType(string transmission)
        {
            if (transmission.Trim() == "")
            {
                return null;
            }

            try
            {
                transmission = EnumHelper.MakeFriendlyString(transmission);
                return (TransmissionType)Enum.Parse(typeof(TransmissionType), transmission);
            }
            catch (Exception)
            {
                throw new Exception("Transmission must be 'automatic' or 'manual'.");
            }
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a FuelType for the Vehicle entity.
        /// Will throw an exception if it cannot resolve the string to the correct enum value.
        /// </summary>
        /// <param name="fuel">A raw string of the fuel not in enum form</param>
        /// <returns>Enum that determines the vehicle's fuel type, or null to use the existing vehicle's fuel type or entity default</returns>
        private FuelType? GetFuelType(string fuel)
        {
            if (fuel.Trim() == "")
            {
                return null;
            }

            try
            {
                fuel = EnumHelper.MakeFriendlyString(fuel);
                return (FuelType)Enum.Parse(typeof(FuelType), fuel);
            }
            catch (Exception)
            {
                throw new Exception("Fuel type must be 'petrol' or 'diesel'.");
            }
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a daily rate double for the Vehicle entity.
        /// Will throw an exception if it cannot convetr the input string to a double.
        /// </summary>
        /// <param name="dailyRate">A raw string of the daily rate</param>
        /// <returns>A double representing the daily rate, or null to use the existing vehicle's daily rate or entity default</returns>
        private double? GetDailyRate(string dailyRate)
        {
            if (dailyRate.Trim() == "")
            {
                return null;
            }

            try
            {
                return double.Parse(dailyRate);
            }
            catch (Exception)
            {
                throw new Exception("Daily rate must be a number greater than 0.");
            }
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into an integer for the Vehicle entity number of seats.
        /// Will throw an exception if it cannot convert the input string into an integer.
        /// </summary>
        /// <param name="numSeats">A raw string of the number of seats</param>
        /// <returns>Integer that represents how many seats the vehicle will have, or null to use the existing
        /// vehicle's value or entity default</returns>
        private int? GetNumSeats(string numSeats)
        {
            if (numSeats.Trim() == "")
            {
                return null;
            }

            try
            {
                return int.Parse(numSeats);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Number of seats must be a number between {0} and {1}.",
                                                    Vehicle.Min_Num_Seats, Vehicle.Max_Num_Seats));
            }
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a boolean for the Vehicle entity GPS property
        /// </summary>
        /// <param name="gps">A raw string representing the GPS boolean</param>
        /// <returns>Boolean that determines whether the vehicle will have GPS or not, or null to use the 
        /// existing vehicle's value or entity default</returns>
        private bool? GetGPS(string gps)
        {
            gps = gps.Trim().ToLower();
            if (gps == "")
            {
                return null;
            }

            return gps == "true";
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a boolean for the Vehicle entity sun roof property
        /// </summary>
        /// <param name="sunRoof">A raw string representing the sunroof boolean</param>
        /// <returns>Boolean that determines whether the vehicle will have a sun roof or not, or null to use the 
        /// existing vehicle's value or entity default</returns>
        private bool? GetSunRoof(string sunRoof)
        {
            sunRoof = sunRoof.Trim().ToLower();
            if (sunRoof == "")
            {
                return null;
            }

            return sunRoof == "true" || sunRoof == "yes";
        }

        /// <summary>
        /// Parses a raw string argument from the VehicleDTO into a string for the Vehicle entity colour property
        /// </summary>
        /// <param name="colour">A raw string representing the desired colour</param>
        /// <returns>String that determines the vehicle's colour, or null to use the existing vehicle's value
        /// or entity default</returns>
        private string GetColour(string colour)
        {
            if (colour == "")
            {
                return null;
            }

            return colour;
        }

        /// <summary>
        /// Determine default value for transmission based on specific vehicle grade
        /// </summary>
        /// <param name="vehicleGrade">Specified grade of vehicle to work from</param>
        /// <returns>Returns correct value based on vehicle grade</returns>
        private TransmissionType GetDefaultTransmissionType(VehicleGrade vehicleGrade)
        {
            if (vehicleGrade == VehicleGrade.Economy)
            {
                return EconomyVehicle.Default_Transmission_Type;
            }
            return Vehicle.Default_Transmission_Type;
        }

        /// <summary>
        /// Determine default value for fuel based on specific vehicle grade
        /// </summary>
        /// <param name="vehicleGrade">Specified grade of vehicle to work from</param>
        /// <returns>Returns correct value based on vehicle grade</returns>
        private FuelType GetDefaultFuelType(VehicleGrade vehicleGrade)
        {
            if (vehicleGrade == VehicleGrade.Commercial)
            {
                return CommercialVehicle.Default_Fuel_Type;
            }
            return Vehicle.Default_Fuel_Type;
        }

        /// <summary>
        /// Determine default value for daily rate based on specific vehicle grade
        /// </summary>
        /// <param name="vehicleGrade">Specified grade of vehicle to work from</param>
        /// <returns>Returns correct value based on vehicle grade</returns>
        private double GetDefaultDailyRate(VehicleGrade vehicleGrade)
        {
            if (vehicleGrade == VehicleGrade.Family)
            {
                return FamilyVehicle.Default_Daily_Rate;
            }
            if (vehicleGrade == VehicleGrade.Commercial)
            {
                return CommercialVehicle.Default_Daily_Rate;
            }
            if (vehicleGrade == VehicleGrade.Luxury)
            {
                return LuxuryVehicle.Default_Daily_Rate;
            }
            return Vehicle.Default_Daily_Rate;
        }

        /// <summary>
        /// Determine default value for GPS based on the specific vehicle grade
        /// </summary>
        /// <param name="vehicleGrade">Specified grade of vehicle to work from</param>
        /// <returns>Returns correct value based on vehicle grade</returns>
        private bool GetDefaultGPS(VehicleGrade vehicleGrade)
        {
            if (vehicleGrade == VehicleGrade.Luxury)
            {
                return LuxuryVehicle.Default_GPS;
            }
            return Vehicle.Default_GPS;
        }

        /// <summary>
        /// Determine default value for sun roof based on the specified vehicle grade
        /// </summary>
        /// <param name="vehicleGrade">Specified grade of vehicle to work from</param>
        /// <returns>Returns correct value based on vehicle grade</returns>
        private bool GetDefaultSunRoof(VehicleGrade vehicleGrade)
        {
            if (vehicleGrade == VehicleGrade.Luxury)
            {
                return LuxuryVehicle.Default_Sun_Roof;
            }
            return Vehicle.Default_Sun_Roof;
        }
    }
}
