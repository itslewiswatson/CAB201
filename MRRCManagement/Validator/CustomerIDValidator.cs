using System;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for customer ID fields
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerIDValidator : InputValidator
    {
        /// <summary>
        /// Validate input is numeric
        /// </summary>
        /// <param name="input">Input to validate</param>
        protected void ValidateNumeric(string input)
        {
            int customerID;
            try
            {
                customerID = int.Parse(input);
            }
            catch (Exception)
            {
                throw new InputInvalidException("Customer ID is not numeric. Please enter a whole number.");
            }
        }

        /// <summary>
        /// Validate that input is positive
        /// </summary>
        /// <param name="input">Input to validate</param>
        protected void ValidatePositive(string input)
        {
            int customerID = int.Parse(input);
            if (customerID < 0)
            {
                throw new InputInvalidException("Customer ID must be greater than 0.");
            }
        }

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateNumeric(input);
            ValidatePositive(input);
        }
    }
}
