using System;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for daily rate fields
    /// Lewis Watson 2020
    /// </summary>
    class DailyRateValidator : InputValidator
    {
        /// <summary>
        /// Validate input can be converted to a double
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateDouble(string input)
        {
            try
            {
                double.Parse(input);
            }
            catch (Exception)
            {
                throw new InputInvalidException("Daily rate must be a positive number (eg: 20, 34.53, 99.9, etc)");
            }
        }

        /// <summary>
        /// Validate input within range
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateRange(string input)
        {
            double dailyRate = double.Parse(input);

            if (dailyRate < 0)
            {
                throw new InputInvalidException("Daily rate must be a positive number (eg: 20, 34.53, 99.9, etc)");
            }
        }

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateDouble(input);
            ValidateRange(input);
        }
    }
}
