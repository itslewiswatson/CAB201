using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Validate the user input in relation to the menu options available
    /// Lewis Watson 2020
    /// </summary>
    public class OptionSelectionValidator : InputValidator
    {
        private List<Option> options;

        public OptionSelectionValidator(List<Option> options)
        {
            this.options = options;
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
                throw new InputInvalidException(string.Format("Input of '{0}' is not a valid option selection", input));
            }
        }

        /// <summary>
        /// Validate that an input is within range
        /// </summary>
        /// <param name="input">Input to validate</param>
        private void ValidateRange(string input)
        {
            int selectedOption = int.Parse(input);
            for (int index = 0; index < options.Count; index++)
            {
                if (index + 1 == selectedOption)
                {
                    return;
                }
            }
            throw new InputInvalidException(string.Format("Your selected option of '{0}' is not valid for this menu", input));
        }

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            ValidateNumeric(input);
            ValidateRange(input);
        }
    }
}
