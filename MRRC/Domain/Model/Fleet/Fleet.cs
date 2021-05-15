using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;

namespace MRRC
{
    /// <summary>
    /// Fleet aggregate
    /// Lewis Watson 2020
    /// </summary>
    public class Fleet : Aggregate
    {
        public List<Vehicle> vehicles { get; }
        public List<Rental> rentals { get; }

        public Fleet(List<Vehicle> vehicles, List<Rental> rentals)
        {
            this.vehicles = vehicles;
            this.rentals = rentals;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (IsVehicleInFleet(vehicle.vehicleRego))
            {
                throw new VehicleAlreadyExistsException(string.Format("vehicleRego: ${0}", vehicle.vehicleRego));
            }
            vehicles.Add(vehicle);
        }

        public Vehicle GetVehicle(string vehicleRego)
        {
            Vehicle vehicle = FindVehicle(vehicleRego);
            if (vehicle == null)
            {
                throw new Exception(string.Format("Vehicle {0} does not exist.", vehicleRego));
            }
            return vehicle;
        }

        public Vehicle FindVehicle(string vehicleRego)
        {
            return vehicles.Find(existingVehicle => existingVehicle.vehicleRego == vehicleRego);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
        }

        public bool IsVehicleInFleet(string vehicleRego)
        {
            return FindVehicle(vehicleRego) != null;
        }

        public bool IsVehicleRented(string vehicleRego)
        {
            return GetCustomerRentingVehicle(vehicleRego) != null;
        }

        public bool IsCustomerRenting(int customerID)
        {
            foreach (Rental rental in rentals)
            {
                if (rental.customerID == customerID)
                {
                    return true;
                }
            }
            return false;
        }

        public int? GetCustomerRentingVehicle(string vehicleRego)
        {
            foreach (Rental rental in rentals)
            {
                if (rental.vehicleRego == vehicleRego)
                {
                    return rental.customerID;
                }
            }
            return null;
        }

        public void RentVehicle(string vehicleRego, int customerID)
        {
            foreach (Rental rental in rentals)
            {
                if (rental.customerID == customerID && rental.vehicleRego == vehicleRego)
                {
                    throw new Exception(string.Format("This customer is already renting this vehicle ({0})", rental.vehicleRego));
                }
                if (rental.customerID == customerID)
                {
                    throw new CustomerCurrentlyRentingException(string.Format("A customer with ID {0} is already renting vehicle {1}",
                                                                rental.customerID, rental.vehicleRego));
                }
                if (rental.vehicleRego == vehicleRego)
                {
                    throw new VehicleCurrentlyRentingException(string.Format("{0} is currently being rented by customer with ID {1}", 
                                                                rental.vehicleRego, rental.customerID));
                }
            }

            Rental newRental = new Rental(vehicleRego, customerID);
            rentals.Add(newRental);
        }

        public void ReturnVehicle(string vehicleRego)
        {
            Rental toRemove = null;

            foreach (Rental rental in rentals)
            {
                if (rental.vehicleRego == vehicleRego)
                {
                    toRemove = rental;
                    break;
                }
            }

            if (toRemove != null)
            {
                rentals.Remove(toRemove);
            }
        }

        public ArrayList GetAttributes()
        {
            ArrayList attributes = new ArrayList();

            foreach (Vehicle vehicle in vehicles)
            {
                List<string> vehicleAttributes = vehicle.GetAttributeList();
                foreach (string attribute in vehicleAttributes)
                {
                    string processedAttribute = attribute.ToUpper();
                    if (!attributes.Contains(processedAttribute))
                    {
                        attributes.Add(processedAttribute);
                    }
                }
            }

            return attributes;
        }
    }
}
