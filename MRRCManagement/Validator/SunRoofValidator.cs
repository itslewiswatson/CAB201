namespace MRRCManagement
{
    /// <summary>
    /// Validate input is yes or no for sun roof fields
    /// Lewis Watson 2020
    /// </summary>
    class SunRoofValidator : YesOrNoValidator
    {
        protected override string GetFieldName()
        {
            return "Sun roof";
        }
    }
}
