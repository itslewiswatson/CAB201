using System;

namespace MRRC
{
    /// <summary>
    /// Conrete implementation of a DTO processor specifically for the rental
    /// Lewis Watson 2020
    /// </summary>
    public class RentalDTOProcessor : DTOPrcocessor<Rental, RentalDTO>
    {
        public Rental ConvertToEntity(RentalDTO objectDTO)
        {
            int customerID = int.Parse((string)objectDTO.customerID);

            return new Rental(objectDTO.vehicleRego, customerID);
        }
    }
}
