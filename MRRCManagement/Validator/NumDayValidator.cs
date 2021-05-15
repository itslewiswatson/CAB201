using System;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for number of day field inputs
    /// </summary>
    public class NumDayValidator : InputValidator
    {
        /// <summary>
        /// Validate that the input is numeric
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateNumeric(string input)
        {
            try
            {
                int.Parse(input);
            }
            catch (Exception)
            {
                throw new InputInvalidException("Number of days must be a valid whole number.");
            }
        }

        /// <summary>
        /// Validate that the input is within a certain range
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateWithinRange(string input)
        {
            int days = int.Parse(input);

            if (days < 1)
            {
                throw new InputInvalidException("Number of days must be greater or equal to 1 full day.");
            }
        }

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateNumeric(input);
            ValidateWithinRange(input);
        }
    }
}
