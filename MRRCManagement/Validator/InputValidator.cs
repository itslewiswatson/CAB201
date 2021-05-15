using System;

namespace MRRCManagement
{
    /// <summary>
    /// Abstract container for validating inputs given from input menus
    /// Lewis Watson 2020
    /// </summary>
    abstract public class InputValidator
    {
        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        abstract public void Validate(string input);

        public bool IsValid(string input)
        {
            input = input.Trim();

            try
            {
                Validate(input);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
