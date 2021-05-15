namespace MRRCManagement
{
    /// <summary>
    /// Menu field abstract class for all other fields to derive basic functionality from
    /// Lewis Watson 2020
    /// </summary>
    abstract public class MenuField
    {
        public string displayText { get; }
        public BasicDisplayable childMenu { get; }

        public MenuField(string displayText, BasicDisplayable childMenu = null)
        {
            this.displayText = displayText;
            this.childMenu = childMenu;
        }
    }
}
