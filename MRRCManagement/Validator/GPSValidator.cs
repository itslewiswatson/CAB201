namespace MRRCManagement
{
    /// <summary>
    /// Validate input for yes or no GPS fields
    /// Lewis Watson 2020
    /// </summary>
    class GPSValidator : YesOrNoValidator
    {
        protected override string GetFieldName()
        {
            return "GPS";
        }
    }
}
