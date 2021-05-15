using System;
using System.Text.RegularExpressions;

namespace MRRCManagement
{
    /// <summary>
    /// Validate input for vehicle rego fields
    /// Lewis Watson 2020
    /// </summary>
    public class VehicleRegoValidator : InputValidator
    {
        private const int Vehicle_Rego_Length = 6;
        private const string Regex_Pattern = @"[0-9]{3}[A-Z]{3}";
        private Regex regex { get; }

        public VehicleRegoValidator()
        {
            regex = new Regex(Regex_Pattern);
        }

        /// <summary>
        /// Validate input string against given regex.
        /// Returns an exception if regex matching times out.
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateRegex(string input)
        {
            try
            {
                regex.Match(input);
            }
            catch (Exception)
            {
                throw new InputInvalidException("An internal error occurred with this registration. Please try another.");
            }
        }

        /// <summary>
        /// Ensures that there is only a singular match in given input and that it is of correct formatting.
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateMatch(string input)
        {
            var matches = regex.Matches(input);
            if (matches.Count != 1)
            {
                throw new InputInvalidException("Please use a registration like 236WVO or 353JAA (consisting of 3 numbers followed by 3 " +
                                                "capital letters).");
            }
        }

        private void ValidateNoExcess(string input)
        {
            input = input.Trim();

            if (input.Length != Vehicle_Rego_Length)
            {
                throw new Exception(string.Format("Please ensure your registration has {0} characters. No more and no less.", Vehicle_Rego_Length));
            }
        }

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateRegex(input);
            ValidateMatch(input);
            ValidateNoExcess(input);
        }
    }
}
