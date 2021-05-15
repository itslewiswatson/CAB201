namespace MRRCManagement
{
    /// <summary>
    /// Validate input for all yes or no fields
    /// Lewis Watson 2020
    /// </summary>
    abstract class YesOrNoValidator : InputValidator
    {
        abstract protected string GetFieldName();

        /// <summary>
        /// Container method for validation
        /// </summary>
        /// <param name="input">Input to validate</param>
        public override void Validate(string input)
        {
            input = input.ToLower();

            if (input != "yes" && input != "no")
            {
                throw new InputInvalidException(string.Format("{0} must be either 'yes' or 'no'", GetFieldName()));
            }
        }
    }
}
