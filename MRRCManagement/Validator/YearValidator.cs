using System;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for year fields.
    /// Lewis Watson 2020
    /// </summary>
    public class YearValidator : InputValidator
    {
        private const int First_Year = 1769; // https://en.wikipedia.org/wiki/Car

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateNumeric(input);
            ValidateRange(input);
        }

        /// <summary>
        /// Validate that input is numeric
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
                throw new InputInvalidException(string.Format("Year must be a an actual year greater than {0}", First_Year));
            }
        }

        /// <summary>
        /// Validate input is within range
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateRange(string input)
        {
            int year = int.Parse(input);
            if (year < First_Year)
            {
                throw new InputInvalidException(string.Format("Year must be a an actual year greater than {0}", First_Year));
            }
        }
    }
}
