namespace MRRCManagement
{
    /// <summary>
    /// Single input contained within an InputMenu
    /// Lewis Watson 2020
    /// </summary>
    public class Input : MenuField
    {
        private InputMenu parentMenu { get; }
        public InputValidator inputValidator { get; }
        private bool allowBlanks { get; } = false;

        public Input(string displayText, InputMenu parentMenu, bool allowBlanks, BasicDisplayable childMenu = null, 
                        InputValidator inputValidator = null) : base(displayText, childMenu)
        {
            this.parentMenu = parentMenu;
            this.inputValidator = inputValidator;
            this.allowBlanks = allowBlanks;
        }
        
        /// <summary>
        /// Validate user-input fields if applicable
        /// </summary>
        /// <param name="input">Raw data entered into input field to validate</param>
        public void ValidateInput(string input)
        {
            if (allowBlanks && input.Trim() == "")
            {
                return;
            }
            if (inputValidator != null)
            {
                inputValidator.Validate(input);
            }
        }
    }
}
