using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Menu of numeric options to select which can reveal other traversal menu options.
    /// Lewis Watson 2020
    /// </summary>
    public class OptionMenu : Menu, Displayable<Option>
    {
        private const string Option_Separator = ")";

        public string header { get; }
        public List<Option> items { get; } = new List<Option>();

        public OptionMenu(Menu parentMenu, string header)
        {
            this.header = header;
            this.parentMenu = parentMenu;
        }

        /// <summary>
        /// Add option to this menu
        /// </summary>
        /// <param name="displayText">Text to display next to numeric option</param>
        /// <param name="childMenu">Menu to traverse into if selected</param>
        public void AddOption(string displayText, BasicDisplayable childMenu = null)
        {
            Option option = new Option(displayText, childMenu);
            items.Add(option);
        }

        /// <summary>
        /// Print all available options
        /// </summary>
        public override void Print()
        {
            PrintOptionsHeader();
            PrintOptions();
        }

        /// <summary>
        /// Print defined header
        /// </summary>
        private void PrintOptionsHeader()
        {
            Console.WriteLine("{0}:", header);
            Console.WriteLine();
        }

        /// <summary>
        /// Print array of options with correct separation and spacing
        /// </summary>
        private void PrintOptions()
        {
            for (var index = 0; index < items.Count; index++)
            {
                Option option = items[index];
                Console.WriteLine("{0}{1} {2}", index + 1, Option_Separator, option.displayText);
            }
            Console.WriteLine();
        }
    }
}
