using System;

namespace MRRC
{
    /// <summary>
    /// Conrete implementation of a DTO processor specifically for the customer
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerDTOProcessor : DTOPrcocessor<Customer, CustomerDTO>
    {
        /// <summary>
        /// Converts a CustomerDTO to a Customer entity
        /// </summary>
        /// <param name="objectDTO">A data-transfer object with all the same basic properties as a Customer</param>
        /// <returns>A fully-fledged customer entity that isn't persisted</returns>
        public Customer ConvertToEntity(CustomerDTO objectDTO)
        {
            int ID;
            Gender gender;
            DateTime dateTime;

            try
            {
                ID = int.Parse(objectDTO.ID);
            } catch (Exception)
            {
                throw new InvalidIDException("ID is not a positive number.");
            }
            
            try
            {
                string genderString = EnumHelper.MakeFriendlyString(objectDTO.gender);
                gender = (Gender)Enum.Parse(typeof(Gender), genderString);
            }
            catch (Exception)
            {
                throw new Exception("Gender must be 'male', 'female', or 'other'");
            }

            try
            {
                dateTime = DateTime.Parse(objectDTO.dateTime);
            }
            catch (Exception)
            {
                throw new Exception("DOB must be a valid date in the form of dd/mm/yyyy");
            }

            Customer customer = new Customer(ID, objectDTO.title, objectDTO.firstName, objectDTO.lastName, gender, dateTime);

            return customer;
        }
    }
}
