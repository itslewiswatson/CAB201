using System;
using System.Collections.Generic;
using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// High-level menu class with common functionality that allows for robust menu traversal
    /// Lewis Watson 2020
    /// </summary>
    abstract public class Menu : BasicDisplayable
    {
        public Menu parentMenu { get; set; }
        private List<Menu> childMenus { get; } = new List<Menu>();

        /// <summary>
        /// Add child input menu
        /// </summary>
        /// <param name="id">Input menu ID to assign to a handler</param>
        /// <param name="header">Display text when entering menu</param>
        /// <returns>Instance of created menu</returns>
        public InputMenu AddInputMenu(string id, string header)
        {
            InputMenu inputMenu = new InputMenu(id, header, this);
            childMenus.Add(inputMenu);
            return inputMenu;
        }

        /// <summary>
        /// Add child option menu
        /// </summary>
        /// <param name="header">Display text when entering menu</param>
        /// <returns>Instance of created menu</returns>
        public OptionMenu AddOptionMenu(string header)
        {
            OptionMenu optionMenu = new OptionMenu(this, header);
            childMenus.Add(optionMenu);
            return optionMenu;
        }

        /// <summary>
        /// Add child table
        /// </summary>
        /// <typeparam name="T">Repository generic type parameter</typeparam>
        /// <typeparam name="U">Repository generic type parameter</typeparam>
        /// <param name="tableType">Type of table to create</param>
        /// <param name="repository">Repository to pass into the table</param>
        /// <returns>Instance of created table</returns>
        public Table AddTable<T, U>(TableType tableType, Repository<T, U> repository)
        {
            if (tableType == TableType.Customer)
            {
                return new CustomerTable(this, (CRMRepository)repository);
            }
            if (tableType == TableType.Fleet)
            {
                return new FleetTable(this, (FleetRepository)repository);
            }
            if (tableType == TableType.Rental)
            {
                return new RentalTable(this, (FleetRepository)repository);
            }

            throw new Exception("Specified table type does not exist");
        }

        /// <summary>
        /// Pass down responsiblity for menus to specifically implement Print()
        /// </summary>
        public abstract void Print();
    }
}
