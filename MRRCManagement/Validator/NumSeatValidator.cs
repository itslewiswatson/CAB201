using MRRC;
using System;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for number of seat inputs
    /// Lewis Watson 2020
    /// </summary>
    class NumSeatValidator : InputValidator
    {
        /// <summary>
        /// Validate that the input is numeric
        /// </summary>
        /// <param name="input">Input to validate</param>
        protected void ValidateNumeric(string input)
        {
            try
            {
                int numSeats = int.Parse(input);
            }
            catch (Exception)
            {
                throw new InputInvalidException(string.Format("Number of seats must be a number between {0} and {1}.", 
                                                Vehicle.Min_Num_Seats, Vehicle.Max_Num_Seats));
            }
        }

        /// <summary>
        /// Validate that input is within given range
        /// </summary>
        /// <param name="input">Input to validate</param>
        protected void ValidateWithinRange(string input)
        {
            int numSeats = int.Parse(input);

            if (numSeats < Vehicle.Min_Num_Seats || numSeats > Vehicle.Max_Num_Seats)
            { 
                throw new InputInvalidException(string.Format("Number of seats must be a number between {0} and {1}.", 
                                                Vehicle.Min_Num_Seats, Vehicle.Max_Num_Seats));
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
