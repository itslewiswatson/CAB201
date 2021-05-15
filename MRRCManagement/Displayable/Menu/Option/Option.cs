namespace MRRCManagement
{
    /// <summary>
    /// Single option contained within an OptionMenu
    /// Lewis Watson 2020
    /// </summary>
    public class Option : MenuField
    {
        public Option(string displayText, BasicDisplayable childMenu = null) : base(displayText, childMenu) { }
    }
}
